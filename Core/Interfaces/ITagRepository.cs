using Core.Entities;

namespace Core.Interfaces
{
    public interface ITagRepository
    {
        Task Add(string tagName);
        Task SubmitCustomHeader(List<Guid> tagIds);
        Task<IEnumerable<Tag>> GetTags();
        Task<IEnumerable<CustomHeader>> GetCustomHeader();
    }
}