using Common;
using Core;
using Core.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class InvestAdRepository(ApplicationDbContext dbContext): IInvestAdRepository
    {
        public async Task<(int count, IEnumerable<InvestAd> list)> Filter(decimal? minUsd,
            decimal? maxUSd,
            decimal? minAnnualInvestmentReturn,
            decimal? maxAnnualInvestmentReturn,
            int page,
            int offset,
            List<Guid>? tagIds)
        {
            var query = dbContext.InvestAds
                .Where(x => x.Published && x.History.Any()); // Ensure there is at least one item in the History list
            if (tagIds?.Count > 0)
                query = query.Where(x => x.Tags.Any(t => tagIds.Contains(t.TagId)));
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
                    .Include(x => x.Tags).ThenInclude(x => x.Tag)
                    .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                    // .ThenInclude(x => x.InvestFields)
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
            return await dbContext.InvestAds
                .Where(x => x.Published)
                .CountAsync();
        }

        public async Task<InvestAd?> Get(Guid id)
        {
            var invest = dbContext.InvestAds
                .Include(x => x.Author)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Where(x => x.Id == id);
            var lastRecord = dbContext.InvestAdExtraInfo
                .Include(x => x.InvestFields)
                .Where(x => x.InvestAdId == id)
                .OrderByDescending(x => x.CreatedAt).Take(1).Include(x => x.AcceptedCurrencies);
            var invTask = await invest.FirstOrDefaultAsync().ConfigureAwait(false);
            if (invTask == null) return null;
            var invHTask = await lastRecord.FirstOrDefaultAsync().ConfigureAwait(false);
            invTask.History = new List<InvestAdExtraInfo>() { invHTask };
            return invTask;
        }
        
        public async Task<InvestAd?> Get(string slug)
        {
            var invest = await dbContext.InvestAds
                .Include(x => x.Author)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Where(x => x.Slug == slug.ToLower()).FirstOrDefaultAsync();
            if (invest == null) return null;
            var lastRecord = await dbContext.InvestAdExtraInfo
                .Include(x => x.InvestFields)
                .Where(x => x.InvestAdId == invest.Id)
                .OrderByDescending(x => x.CreatedAt).Take(1).Include(x => x.AcceptedCurrencies)
                .FirstOrDefaultAsync();
            invest.History = new List<InvestAdExtraInfo>() { lastRecord };
            return invest;
        }

        private async Task<InvestAd?> GetRaw(Guid id) => await dbContext.InvestAds
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task Create(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo)
        {
            investAd.History = new List<InvestAdExtraInfo>() { investAdExtraInfo };
            investAd.Id = Guid.NewGuid();
            _ = await dbContext.InvestAds.AddAsync(investAd);
            await dbContext.SaveChangesAsync();
        }

        public async Task Edit(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo)
        {
            var inv = await GetRaw(investAd.Id);
            if (inv != null)
            {
                inv.Published = investAd.Published;
                inv.Tags = investAd.Tags;
                investAdExtraInfo.InvestAdId = investAd.Id;
                await dbContext.InvestAdExtraInfo.AddAsync(investAdExtraInfo);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Attempt to modify unexisting object");
            }
        }

        public async Task<IEnumerable<InvestAdExtraInfo>> Search(string searchTerm, int currentPage, int itemsPerPage)
        {
            var groupedQuery = dbContext.InvestAdExtraInfo
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
            return await dbContext.InvestAds.AnyAsync(x => x.Id == Guid.Parse(invId) && x.AuthorId == userId);
        }

        public async Task<IEnumerable<InvestAd>> GetSimilarInvest(List<Guid> tagIds)
        {
            return await dbContext.InvestAds
                .Where(x => x.Tags.Any(
                    t => tagIds.Any(pt => pt == t.TagId))
                ).OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.History.OrderByDescending(y => y.CreatedAt).Take(1))
                .ToListAsync();
        }
    }
}