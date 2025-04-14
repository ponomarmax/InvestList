using Core.Entities;
using Radar.Domain.Entities;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task PublishAsync(PostComment comment);
    }
}