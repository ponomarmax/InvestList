using Core.Entities;

namespace Core.Interfaces
{
    public interface ITagRepository
    {
        Task Add(string tagName);
        Task<Dictionary<Guid, string>> GetTags();
        Task SubmitCustomHeader(List<Guid> tagIds);
        Task<IEnumerable<Tag>> GetTagsV2();
        Task<IEnumerable<CustomHeader>> GetCustomHeader();
    }
}