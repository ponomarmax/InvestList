using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface INewsRepository
    {
        Task<News?> Get(Guid id);
        Task Create(News news);
        Task<IEnumerable<News>> GetPage(int page, int itemsPerPage);
        Task<int> Count();
        Task<Dictionary<Guid, string>> GetTags();
        Task<IEnumerable<News>> GetSimilarNews(Guid id);
    }
}
