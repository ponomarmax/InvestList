using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class InvestAdRepository: IInvestAdRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InvestAdRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<InvestAd>> GetAllShorted(int page = 1, int offset = 10)
        {
            return await _dbContext.InvestAds
                //.Where(x => x.Published)
                //.OrderBy(x => x.CreatedAt)
                //.Skip((page - 1) * offset)
                //.Take(offset)
                .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.InvestFields)
                .ToListAsync();
        }

        public async Task<IEnumerable<InvestField>> GetFields()
        {
            return await _dbContext.InvestFields.ToArrayAsync();
        }

        public async Task Create(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo)
        {
            investAd.History = new List<InvestAdExtraInfo>() { investAdExtraInfo };
            _ = await _dbContext.InvestAds.AddAsync(investAd);
            await _dbContext.SaveChangesAsync();
        }
    }
}
