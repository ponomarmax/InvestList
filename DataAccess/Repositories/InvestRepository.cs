using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class InvestRepository(ApplicationDbContext dbContext, IMapper mapper, IImageService imageService): IInvestRepository
    {
        public async Task<(int count, IEnumerable<InvestPost> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds,
            string search = null)
        {
            var query = dbContext.InvestPosts
                .Where(x => x.Post.IsActive && x.Post.PostType == PostType.InvestAd).AsNoTracking();
            if (tagIds?.Count() > 0)
                query = query.Where(x => x.Post.Tags.Any(t => tagIds.Contains(t.TagId)));

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Post.Title.Contains(search));
            
            var count = await query.CountAsync();

            if (count > 0)
            {
                return (count, await query
                    .OrderByDescending(x => x.Post.Priority)
                    .ThenByDescending(x => x.Post.CreatedAt)
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .Include(x => x.Post.ImagesV2)
                    .Include(x => x.Post.CreatedBy)
                    .Include(x => x.Post.Tags).ThenInclude(x => x.Tag)
                    .Include(x => x.MinInvestValues)
                    .Include(x=>x.Post.GoogleAnalyticPostView)
                    .ToListAsync());
            }

            return (0, Array.Empty<InvestPost>());
        }

        public async Task<InvestPost?> Get(string slug)
        {
            var post = await dbContext.Posts
                .Include(x => x.ImagesV2)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Where(x => x.Slug == slug.ToLower() && x.PostType == PostType.InvestAd).FirstOrDefaultAsync();
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
                .Include(x => x.ImagesV2).ThenInclude(x=>x.ImageObject)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && x.PostType == PostType.InvestAd);
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
            var oldImagePaths = postOrigin.Post.ImagesV2.Select(x => x.Id);
            if (postOrigin != null)
            {
                mapper.Map(invest, postOrigin);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }
            postOrigin = await Get(id);
            imageService.RefreshImages(postOrigin.Post, oldImagePaths);
        }

        public async Task Create(InvestPost invest)
        {
            dbContext.InvestPosts.Add(invest);
            await dbContext.SaveChangesAsync();
            var postOrigin = await Get(invest.Post.Slug);
            imageService.RefreshImages(postOrigin.Post, null);
        }
    }
}