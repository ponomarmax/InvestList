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
                .Where(x => x.Published)
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

        public async Task<(int count, IEnumerable<InvestAd> list)> Filter(decimal? minUsd,
            decimal? maxUSd,
            decimal? minAnnualInvestmentReturn,
            decimal? maxAnnualInvestmentReturn,
            int page,
            int offset)
        {
            var query = _dbContext.InvestAds
                .Where(x => x.Published && x.History.Any()); // Ensure there is at least one item in the History list

            // Apply other filters as needed
            //if (minUsd.HasValue)
            //{
            //    query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AcceptedCurrencies.Any(c => c.Currency == Currency.USD && c.MinValue >= minUsd.Value));
            //}

            //if (maxUSd.HasValue)
            //{
            //    query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AcceptedCurrencies.Any(c => c.Currency == Currency.USD && c.MinValue <= maxUSd.Value));
            //}
            var (uahMin, uahMax) = getUahRange(minUsd, maxUSd);

            //if (uahMin.HasValue)
            //{
            //    query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AcceptedCurrencies.Any(c => c.Currency == Currency.UAH && c.MinValue >= uahMin.Value));
            //}

            //if (uahMax.HasValue)
            //{
            //    query = query.Where(x => x.History.OrderByDescending(y => y.CreatedAt).First().AcceptedCurrencies.Any(c => c.Currency == Currency.UAH && c.MinValue <= uahMax.Value));
            //}

            bool hasUsdFilter = minUsd.HasValue || maxUSd.HasValue;
            bool hasUahFilter = uahMin.HasValue || uahMax.HasValue;

            if (hasUsdFilter || hasUahFilter)

                query = query.Where(x =>
                    x.History.OrderByDescending(y => y.CreatedAt)
                        .First()
                        .AcceptedCurrencies.Any(c =>
                            ((hasUsdFilter && c.Currency == Currency.USD) ||
                             (hasUahFilter && c.Currency == Currency.UAH)) &&
                            (
                                (hasUsdFilter &&
                                 (minUsd == null || c.Currency == Currency.USD && c.MinValue >= minUsd.Value) &&
                                 (maxUSd == null || c.Currency == Currency.USD && c.MinValue <= maxUSd.Value)) ||
                                (hasUahFilter &&
                                 (uahMin == null || c.Currency == Currency.UAH && c.MinValue >= uahMin.Value) &&
                                 (uahMax == null || c.Currency == Currency.UAH && c.MinValue <= uahMax.Value))
                            )
                        )
                );

            if (minAnnualInvestmentReturn.HasValue)
            {
                query = query.Where(x =>
                    x.History.OrderByDescending(y => y.CreatedAt).First().AnnualInvestmentReturn >=
                    minAnnualInvestmentReturn.Value);
            }

            if (maxAnnualInvestmentReturn.HasValue)
            {
                query = query.Where(x =>
                    x.History.OrderByDescending(y => y.CreatedAt).First().AnnualInvestmentReturn <=
                    maxAnnualInvestmentReturn.Value);
            }

            var count = await query.CountAsync();

            if (count > 0)
            {
                return (count, await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * offset)
                    .Take(offset)
                    .Include(x => x.Author)
                    .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.InvestFields)
                    .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    .ThenInclude(x => x.AcceptedCurrencies)
                    .ToListAsync());
            }

            return (0, Array.Empty<InvestAd>());
        }

        private (decimal? minUsd, decimal? maxUSd) getUahRange(decimal? min, decimal? max)
        {
            return (min * 37, max * 37);
        }

        public async Task<int> Count()
        {
            return await _dbContext.InvestAds
                .Where(x => x.Published)
                .CountAsync();
        }

        public async Task<InvestAd?> Get(Guid id)
        {
            var invest = _dbContext.InvestAds
                .Include(x => x.Author)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Where(x => x.Id == id);
            var lastRecord = _dbContext.InvestAdExtraInfo
                .Include(x => x.InvestFields)
                .Where(x => x.InvestAdId == id)
                .OrderByDescending(x => x.CreatedAt).Take(1).Include(x => x.AcceptedCurrencies);
            var invTask = await invest.FirstOrDefaultAsync().ConfigureAwait(false);
            if (invTask == null) return null;
            var invHTask = await lastRecord.FirstOrDefaultAsync().ConfigureAwait(false);
            invTask.History = new List<InvestAdExtraInfo>() { invHTask };
            return invTask;
        }

        private async Task<InvestAd?> GetRaw(Guid id) => await _dbContext.InvestAds
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
            var inv = await GetRaw(investAd.Id);
            if (inv != null)
            {
                inv.Published = investAd.Published;
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