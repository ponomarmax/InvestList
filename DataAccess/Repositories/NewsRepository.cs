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

        public async Task<int> Count()
        {
            return await _dbContext.InvestAds
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

        public async Task<IEnumerable<News>> GetPage(int page, int itemsPerPage)
        {
            return await _dbContext.News
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<News>> GetSimilarNews(Guid id)
        {
            var primaryNews = _dbContext.News.Where(x => x.Id == id).Include(x => x.Tags).FirstOrDefault();
            var tagIds = primaryNews.Tags.Select(x => x.TagId);
            return await _dbContext.News
                .Where(x => x.Id != id && 
                            x.Tags.Any(
                                t => tagIds.Any(pt => pt == t.TagId))
                ).OrderByDescending(x=>x.CreatedAt)
                .Take(10)
                .Include(x=>x.Tags).ThenInclude(x=>x.Tag)
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