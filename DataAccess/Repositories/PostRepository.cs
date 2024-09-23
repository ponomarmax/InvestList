using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PostRepository(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IImageService imageService
    )
        : IPostRepository
    {
        public async Task<(int count, IEnumerable<Post> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds,
            string search = null,
            PostType? type = null)
        {
            var query = dbContext.Posts.AsNoTracking()
                .Where(x => x.IsActive);

            if (type != null)
                query = query.Where(x => x.PostType == type);

            if (tagIds?.Count() > 0)
                query = query.Where(x => x.Tags.Any(t => tagIds.Contains(t.TagId)));
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Title.Contains(search));

            var count = await query.CountAsync();

            if (count > 0)
            {
                var listAsync = await query
                    .OrderByDescending(x => x.Priority)
                    .ThenByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .Include(x => x.ImagesV2)
                    .Include(x => x.CreatedBy)
                    .Include(x => x.Tags).ThenInclude(x => x.Tag)
                    .AsSplitQuery()
                    // .Select(x => new Post()
                    // {
                    //     Id = x.Id,
                    //     Title = x.Title,
                    //     TitleSeo = x.TitleSeo,
                    //     PostType = x.PostType,
                    //     Slug = x.Slug,
                    //     IsActive = x.IsActive,
                    //     CreatedAt = x.CreatedAt,
                    //     UpdatedAt = x.UpdatedAt,
                    //     CreatedById = x.CreatedById,
                    //     DescriptionSeo = x.DescriptionSeo,
                    //     Description = x.Description,
                    //     Images = x.Images.Select(x => new Image
                    //     {
                    //         Id = x.Id,
                    //         // ImageBase64 = x.ImageBase64,
                    //         // this is excluded on purpose due to perfomance issue
                    //         // it takes lots of time to dispose this large string.
                    //         PostId = x.PostId
                    //     }),
                    //     Tags = x.Tags.Select(x => new PostTags()
                    //     {
                    //         PostId = x.PostId, TagId = x.TagId, Tag = new Tag { Name = x.Tag.Name, Id = x.TagId }
                    //     })
                    // })
                    .ToListAsync();
                return (count, listAsync);
            }

            return (0, Array.Empty<Post>());
        }

        public async Task<Post?> Get(string id, PostType? postType = null)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var post = Queryable.AsQueryable(dbContext.Posts);
            post = Guid.TryParse(id, out var idGuid)
                ? post.Where(x => x.Id == idGuid)
                : post.Where(x => x.Slug == id);

            if (postType != null)
                post = post.Where(x => x.PostType == postType);

            return await post
                .Include(x => x.ImagesV2).ThenInclude(x => x.ImageObject)
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
            var oldImagePaths = postOrigin.ImagesV2.Select(x => x.Id);
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
            return await dbContext.Posts.Where(x => x.PostType == postType).CountAsync();
        }
    }
}