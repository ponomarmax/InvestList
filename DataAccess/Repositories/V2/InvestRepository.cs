using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.V2
{
    public interface IInvestRepository
    {
        Task<InvestPost?> Get(string slug);
        Task<InvestPost?> Get(Guid id);
        Task Put(Guid id, InvestPost invest);
        Task<IEnumerable<Post>> GetSimilarInvests(Guid id, List<Guid> tagIds);
        Task Create(InvestPost invest);
        Task<bool> Exists(string slug);

        Task<(int count, IEnumerable<InvestPost> list)> Filter(
            int page,
            int offset,
            IEnumerable<Guid>? tagIds);
    }

    public class InvestRepository(ApplicationDbContext dbContext, IMapper mapper): IInvestRepository
    {
        public async Task<(int count, IEnumerable<InvestPost> list)> Filter(
            int page,
            int offset,
            IEnumerable<Guid>? tagIds)
        {
            var query = dbContext.InvestPosts
                .Where(x => x.Post.IsActive); 
            if (tagIds?.Count() > 0)
                query = query.Where(x => x.Post.Tags.Any(t => tagIds.Contains(t.TagId)));
        
            var count = await query.CountAsync();
        
            if (count > 0)
            {
                return (count, await query
                    .OrderByDescending(x => x.Post.CreatedAt)
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .Include(x=>x.Post.Images)
                    .Include(x => x.Post.CreatedBy)
                    .Include(x => x.Post.Tags).ThenInclude(x => x.Tag)
                    .Include(x => x.MinInvestValues)
                    .ToListAsync());
            }
        
            return (0, Array.Empty<InvestPost>());
        }
        
        public async Task<InvestPost?> Get(string slug)
        {
            var post = await dbContext.Posts
                .Include(x => x.Images)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Where(x => x.Slug == slug.ToLower()).FirstOrDefaultAsync();
            if (post == null) return null;
            var relatedInfo = await dbContext.InvestPosts
                .Include(x => x.MinInvestValues)
                .FirstOrDefaultAsync(x => x.PostId == post.Id);
            relatedInfo.Post = post;
            return relatedInfo;
        }

        public async Task<bool> Exists(string slug)
        {
            return await dbContext.Posts.CountAsync(x => x.Slug == slug.ToLower()) > 0;
        }


        public async Task<InvestPost?> Get(Guid id)
        {
            var post = await dbContext.Posts
                .Include(x => x.Images)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (post == null) return null;
            var relatedInfo = await dbContext.InvestPosts
                .Include(x => x.MinInvestValues)
                .FirstOrDefaultAsync(x => x.PostId == post.Id);
            relatedInfo.Post = post;
            return relatedInfo;
        }

        public async Task Put(Guid id, InvestPost invest)
        {
            var postOrigin = await Get(id);
            if (postOrigin != null)
            {
                mapper.Map(invest, postOrigin);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }
        }

        public async Task Create(InvestPost invest)
        {
            dbContext.InvestPosts.Add(invest);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetSimilarInvests(Guid id, List<Guid> tagIds)
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