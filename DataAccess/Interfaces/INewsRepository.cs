using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface INewsRepository
    {
        Task Edit(News news);
        Task<News?> Get(Guid id);
        Task Create(News news);
        Task<IEnumerable<News>> GetPage(int page, int itemsPerPage, List<Guid>? tagIds);
        Task<int> Count(List<Guid>? tagIds = null); 
        Task<IEnumerable<News>> GetSimilarNews(List<Guid> tagIds);
    }
}
