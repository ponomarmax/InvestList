using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IInvestAdRepository
    {
        Task Create(InvestAd investAd, InvestAdExtraInfo investAdExtraInfo);
        Task<IEnumerable<InvestAd>> GetAllShorted(int page = 1, int offset = 0);
        Task<IEnumerable<InvestField>> GetFields();
    }
}