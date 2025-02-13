using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Radar.Domain;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;
using Radar.EF.Repositories;
using IImageService = Core.Interfaces.IImageService;

namespace DataAccess.Repositories
{
    public class PostRepository(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IImageService imageService
    )
        : BasePostRepository<Post>(dbContext), IPostRepository
    {
        public async Task<(int count, IEnumerable<Post> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds,
            string search = null,
            PostType? type = null)
        {
            var request = new PaginationData()
            {
                Page = page,
                Language = "ua",
                PostType = Enum.GetName(typeof(PostType), type),
                Search = search,
                TagsIds = tagIds?.ToList(),
                Take = offset
            };
            return await Filter(request);
        }

        public async Task<Post?> Get(string id, PostType? postType = null)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var post = Queryable.AsQueryable(dbContext.Posts);
            post = Guid.TryParse(id, out var idGuid)
                ? post.Where(x => x.Id == idGuid)
                : post.Where(x => x.Slug == id);

            if (postType != null)
                post = post.Where(x => x.PostType == postType.ToString());

            return await post
                .Include(x => x.Images).ThenInclude(x => x.ImageObject)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Include(x => x.Links)
                .AsSplitQuery().FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Post>> GetSimilarPosts(Guid id, List<Guid> tagIds)
        {
            return await dbContext.Posts
                .Where(x => x.Id != id && x.Tags.Any(
                    t => tagIds.Any(pt => pt == t.TagId))
                ).OrderByDescending(x => x.CreatedAt)
                .Take(100)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsWithLastComments()
        {
            var postsWithLatestComments = dbContext.Posts
                .Where(x => x.Comments.Any())
                .Select(post => new
                {
                    Post = post,
                    LatestComment = post.Comments
                        .OrderByDescending(comment => comment.CreatedAt)
                        .Select(comment => new
                        {
                            Comment = comment,
                            User = comment.User
                        })
                        .FirstOrDefault(),
                })
                .OrderByDescending(x => x.LatestComment.Comment.CreatedAt)
                .Take(4);
            var pos = await postsWithLatestComments.ToListAsync();

            var result = pos.Select(x =>
            {
                var post = x.Post;
                if (x.LatestComment == null) return post;

                var latestComment = x.LatestComment.Comment;
                latestComment.User = x.LatestComment.User;
                post.Comments = new List<PostComment> { latestComment };

                return post;
            }).ToList();

            return result;
        }

        public async Task Put(Guid id, Post post)
        {
            var postOrigin = await Get(id.ToString());
            var oldImagePaths = postOrigin.Images.Select(x => x.Id);
            if (postOrigin != null)
            {
                mapper.Map(post, postOrigin);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }

            postOrigin = await Get(id.ToString());
            imageService.RefreshImages(postOrigin, oldImagePaths);
        }

        public async Task SetPriority(Guid id, int priority)
        {
            var postOrigin = await Get(id.ToString());
            if (postOrigin != null)
            {
                postOrigin.Priority = priority;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }
        }


        public async Task Create(Post post)
        {
            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();
            var postOrigin = await Get(post.Slug);
            imageService.RefreshImages(postOrigin, null);
        }

        public async Task<bool> Exists(string slug)
        {
            return await dbContext.Posts.CountAsync(x => x.Slug == slug.ToLower()) > 0;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await dbContext.Posts.CountAsync(x => x.Id == id) > 0;
        }

        public async Task<bool> IsOwnerOfPost(string userId, string postId)
        {
            return await dbContext.Posts.AnyAsync(x => x.Id == Guid.Parse(postId) && x.CreatedById == userId);
        }

        public async Task<int> Count(PostType? postType)
        {
            if (postType == null)
                return await dbContext.Posts.CountAsync();
            return await dbContext.Posts.Where(x => x.PostType == postType.ToString()).CountAsync();
        }
    }
}