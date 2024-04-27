using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class NewsRepository: INewsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<News?> GetRaw(Guid id) => await _dbContext.News
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task Edit(News news)
        {
            var inv = await GetRaw(news.Id);
            if (inv != null)
            {
                inv.Title = news.Title;
                inv.Description = news.Description;
                inv.ImageBase64 = news.ImageBase64;
                inv.Tags = news.Tags;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }
        }

        public async Task<int> Count(List<Guid>? tagIds = null)
        {
            return await GetNewsEnumerable(tagIds)
                .CountAsync();
        }

        public async Task<Dictionary<Guid, string>> GetTags()
        {
            return (await _dbContext.Tags.ToListAsync()).ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task Create(News news)
        {
            await _dbContext.News.AddAsync(news);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<News?> Get(Guid id)
        {
            return await _dbContext.News.Include(x => x.Author)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<News>> GetPage(int page, int itemsPerPage, List<Guid>? tagIds)
        {
            var newsEnumerable = GetNewsEnumerable(tagIds);

            return await newsEnumerable
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();
        }

        private IQueryable<News> GetNewsEnumerable(List<Guid>? tagIds)
        {
            var newsEnumerable = _dbContext.News.AsQueryable();
            if (tagIds?.Count > 0)
                newsEnumerable = newsEnumerable.Where(x => x.Tags.Any(t => tagIds.Contains(t.TagId)));
            return newsEnumerable;
        }

        public async Task<IEnumerable<News>> GetSimilarNews(Guid id)
        {
            var primaryNews = _dbContext.News.Where(x => x.Id == id).Include(x => x.Tags).FirstOrDefault();
            var tagIds = primaryNews.Tags.Select(x => x.TagId);
            return await _dbContext.News
                .Where(x => x.Id != id &&
                            x.Tags.Any(
                                t => tagIds.Any(pt => pt == t.TagId))
                ).OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .ToListAsync();
            // .OrderByDescending(x => x.CreatedAt)
            // .Skip((page - 1) * itemsPerPage)
            // .Take(itemsPerPage)
            // .Include(x => x.Author)
            // .Include(x => x.Tags)
            // .ThenInclude(x => x.Tag)
            // .ToListAsync();
        }
    }
}