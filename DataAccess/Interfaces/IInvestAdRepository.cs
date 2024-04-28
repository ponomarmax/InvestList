using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IInvestAdRepository
    {
        Task<int> Count();
        Task Create(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo);
        Task Edit(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo);
        Task<(int count, IEnumerable<InvestAd> list)> Filter(decimal? minUsd, decimal? maxUSd, decimal? minAnnualInvestmentReturn, decimal? maxAnnualInvestmentReturn, int page, int offset);
        Task<InvestAd?> Get(Guid id);
        Task<IEnumerable<InvestAdExtraInfo>> Search(string searchTerm, int currentPage, int itemsPerPage);
        Task<bool> IsOwnerOfPost(string userId, string invId);
    }
}