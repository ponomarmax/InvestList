using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.V2;

namespace DataAccess.Repositories
{
    public class CommentRepository(
        ApplicationDbContext dbContext,
        IUserRepository userRepository,
        IPostRepository repository)
        : ICommentRepository
    {
        public async Task PublishAsync(PostComment comment)
        {
            _ = await userRepository.Get(comment.UserId) ?? throw new NullReferenceException("User not found");
            _ = await repository.Get(comment.PostId.ToString()) ?? throw new NullReferenceException("Ads not found");
            comment.CreatedAt = DateTime.UtcNow;
            dbContext.PostComments.Add(comment);
            await dbContext.SaveChangesAsync();
        }
    }
}