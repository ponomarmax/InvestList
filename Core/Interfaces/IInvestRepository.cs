using Core.Entities;

namespace Core.Interfaces
{
    public interface IInvestRepository
    {
        Task<InvestPost?> Get(string slug);
        Task<InvestPost?> Get(Guid id);
        Task Put(Guid id, InvestPost invest);
        Task Create(InvestPost invest);
        Task<bool> Exists(string slug);

        Task<(int count, IEnumerable<InvestPost> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds);
    }
}