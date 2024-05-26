using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Task PublishAsync(PostComment comment);
    }
}