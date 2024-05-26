using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.V2
{
    public interface IPostRepository
    {
        Task<(int count, IEnumerable<Post> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds);

        Task<Post?> Get(string id);
        Task<IEnumerable<Post>> GetSimilarPosts(Guid id, List<Guid> tagIds);
        Task Put(Guid id, Post post);
        Task Create(Post post);
        Task<bool> Exists(string slug);
    }

    public class PostRepository(ApplicationDbContext dbContext, IMapper mapper): IPostRepository
    {
        public async Task<(int count, IEnumerable<Post> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds)
        {
            var query = dbContext.Posts.AsNoTracking()
                .Where(x => x.IsActive && x.PostType == PostType.News);
            if (tagIds?.Count() > 0)
                query = query.Where(x => x.Tags.Any(t => tagIds.Contains(t.TagId)));

            var count = await query.CountAsync();

            if (count > 0)
            {
                var listAsync = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .Include(x => x.Images)
                    .Include(x => x.CreatedBy)
                    .Include(x => x.Tags).ThenInclude(x => x.Tag)
                    .AsSplitQuery()
                    .Select(x => new Post()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        TitleSeo = x.TitleSeo,
                        PostType = x.PostType,
                        Slug = x.Slug,
                        IsActive = x.IsActive,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        CreatedById = x.CreatedById,
                        DescriptionSeo = x.DescriptionSeo,
                        Description = x.Description,
                        Images = x.Images.Select(x => new Image
                        {
                            Id = x.Id,
                            // ImageBase64 = x.ImageBase64,
                            // this is excluded on purpose due to perfomance issue
                            // it takes lots of time to dispose this large string.
                            PostId = x.PostId
                        }),
                        Tags = x.Tags.Select(x => new PostTags()
                        {
                            PostId = x.PostId, TagId = x.TagId, Tag = new Tag { Name = x.Tag.Name, Id = x.TagId }
                        })
                    })
                    .ToListAsync();
                return (count, listAsync);
            }

            return (0, Array.Empty<Post>());
        }

        public async Task<Post?> Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var post = dbContext.Posts
                .Include(x => x.Images)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Include(x => x.Links)
                .AsSplitQuery();

            if (Guid.TryParse(id, out var idGuid))
                return await post.FirstOrDefaultAsync(x => x.Id == idGuid && x.PostType == PostType.News);

            return await post.FirstOrDefaultAsync(x => x.Slug == id && x.PostType == PostType.News);
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

        public async Task Put(Guid id, Post post)
        {
            var postOrigin = await Get(id.ToString());
            if (postOrigin != null)
            {
                mapper.Map(post, postOrigin);
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
        }

        public async Task<bool> Exists(string slug)
        {
            return await dbContext.Posts.CountAsync(x => x.Slug == slug.ToLower()) > 0;
        }
    }
}