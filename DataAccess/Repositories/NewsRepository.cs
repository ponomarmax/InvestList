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

        public async Task Create(News news)
        {
            await _dbContext.News.AddAsync(news);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<News?> Get(Guid id)
        {
            return await _dbContext.News.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<News>> GetPage(int page, int itemsPerPage)
        {
            return await _dbContext.News
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Include(x => x.Author)
                .ToListAsync();
        }
    }
}
