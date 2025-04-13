using Core.Entities;
using Radar.Domain.Entities;

namespace Core.Interfaces
{
    public interface ITagRepository
    {
        Task Add(string tagName);
        Task SubmitCustomHeader(List<Guid> tagIds);
        Task<IEnumerable<CustomHeader>> GetCustomHeader(string language);
    }
}