using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IInvestAdRepository
    {
        Task<int> Count();
        Task Create(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo);
        Task Edit(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo);
        Task<InvestAd?> Get(Guid id);
        Task<IEnumerable<InvestAd>> GetAllShorted(int page = 1, int offset = 0);
        Task<IEnumerable<InvestField>> GetFields();
    }
}