using Core.Entities;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task PublishAsync(PostComment comment);
    }
}