using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Task PublishAsync(Comment comment);
        Task PublishAsync(PostComment comment);
    }
}