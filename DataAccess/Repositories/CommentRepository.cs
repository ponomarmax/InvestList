using Core.Entities;
using Core.Interfaces;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;

namespace DataAccess.Repositories
{
    public class CommentRepository(
        ApplicationDbContext dbContext,
        IUserRepository userRepository,
        IBasePostRepository repository)
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