using Core.Entities;
using Radar.Domain;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;

namespace Core.Interfaces
{
    public interface IInvestRepository:IBasePostRepository
    {
        Task<InvestPost?> Get(string slug);
        Task<InvestPost?> Get(Guid id);
        Task<string> Put(Guid? id, InvestPost invest);
        Task<string> Create(InvestPost invest);
        Task<bool> Exists(string slug);

        Task<(int count, IEnumerable<InvestPost> list)> Filter(PaginationData paginationData);

        Task<Dictionary<string, List<(Post Post, InvestPost? Invest)>>> GetGroupedPostsWithInvestAsync(
            string language,
            string? search,
            List<Guid>? tagIds,
            CancellationToken ct);
    }
}