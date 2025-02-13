using Core.Entities;
using Core.Interfaces;
using Radar.Domain.Entities;

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
            if (!await repository.Exists(comment.PostId))
                throw new NullReferenceException("Ads not found");
            comment.CreatedAt = DateTime.UtcNow;
            dbContext.PostComments.Add(comment);
            await dbContext.SaveChangesAsync();
        }
    }
}