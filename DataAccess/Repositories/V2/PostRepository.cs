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
    }

    public class PostRepository(ApplicationDbContext dbContext, IMapper mapper): IPostRepository
    {
        public async Task<(int count, IEnumerable<Post> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds)
        {
            var query = dbContext.Posts
                .Where(x => x.IsActive && x.PostType == PostType.News);
            if (tagIds?.Count() > 0)
                query = query.Where(x => x.Tags.Any(t => tagIds.Contains(t.TagId)));

            var count = await query.CountAsync();

            if (count > 0)
            {
                return (count, await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .Include(x => x.Images)
                    .Include(x => x.CreatedBy)
                    .Include(x => x.Tags).ThenInclude(x => x.Tag)
                    .ToListAsync());
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
                .Include(x => x.Comments).ThenInclude(x => x.User);

            if (Guid.TryParse(id, out var idGuid))
                return await post.Where(x => x.Id == idGuid).FirstOrDefaultAsync();

            return await post.Where(x => x.Slug == id).FirstOrDefaultAsync();
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
    }
}