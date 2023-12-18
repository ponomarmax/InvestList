using Common;
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
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * offset)
                .Take(offset)
                .Include(x => x.Author)
                .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.InvestFields)
                 .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.AcceptedCurrencies)
                .ToListAsync();
        }

        public async Task<IEnumerable<InvestAd>> Filter(decimal? minUsd, decimal? maxUSd, decimal? minAnnualInvestmentReturn, decimal? maxAnnualInvestmentReturn, int page, int offset)
        {
            var query = _dbContext.InvestAds
                .Where(x => x.History.Any()); // Ensure there is at least one item in the History list

            // Apply other filters as needed
            if (minUsd.HasValue)
            {
                query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AcceptedCurrencies.Any(c => c.Currency == Currency.USD && c.MinValue >= minUsd.Value));
            }

            if (maxUSd.HasValue)
            {
                query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AcceptedCurrencies.Any(c => c.Currency == Currency.USD && c.MinValue <= maxUSd.Value));
            }

            if (minAnnualInvestmentReturn.HasValue)
            {
                query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AnnualInvestmentReturn >= minAnnualInvestmentReturn.Value);
            }

            if (maxAnnualInvestmentReturn.HasValue)
            {
                query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AnnualInvestmentReturn <= maxAnnualInvestmentReturn.Value);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * offset)
                .Take(offset)
                .Include(x => x.Author)
                .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.InvestFields)
                 .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.AcceptedCurrencies)
                .ToListAsync();
        }

        public async Task<int> Count()
        {
            return await _dbContext.InvestAds
                //.Where(x => x.Published)
                .CountAsync();
        }

        public async Task<InvestAd?> Get(Guid id) => await _dbContext.InvestAds
                .Include(x => x.Author)
                .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.AcceptedCurrencies)
                .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.InvestFields).ThenInclude(x => x.InvestField)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> Contains(InvestAd ad) => await _dbContext.InvestAds.ContainsAsync(ad);

        public async Task<IEnumerable<InvestField>> GetFields()
        {
            return await _dbContext.InvestFields.ToArrayAsync();
        }

        public async Task Create(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo)
        {
            investAd.History = new List<InvestAdExtraInfo>() { investAdExtraInfo };
            investAd.Id = Guid.NewGuid();
            _ = await _dbContext.InvestAds.AddAsync(investAd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo)
        {
            if (await Contains(investAd))
            {
                investAdExtraInfo.InvestAdId = investAd.Id;
                await _dbContext.InvestAdExtraInfo.AddAsync(investAdExtraInfo);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }
        }

        public async Task<IEnumerable<InvestAdExtraInfo>> Search(string searchTerm, int currentPage, int itemsPerPage)
        {
            var groupedQuery = _dbContext.InvestAdExtraInfo
                .GroupBy(i => i.InvestAdId)
                .Select(g => g
                    .OrderByDescending(e => e.CreatedAt)
                    .FirstOrDefault()).ToList();

            // Apply search criteria to the grouped query
            var filteredQuery = groupedQuery
     .Where(e0 =>
         (e0.Title.Contains(searchTerm)) ||
         (e0.Description != null && e0.Description.Contains(searchTerm)));


            // Apply pagination
            var r = filteredQuery
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            return r;
        }

        public async Task<bool> IsOwnerOfPost(string userId, string invId)
        {
            return await _dbContext.InvestAds.AnyAsync(x => x.Id == Guid.Parse(invId) && x.AuthorId == userId);
        }
    }
}
