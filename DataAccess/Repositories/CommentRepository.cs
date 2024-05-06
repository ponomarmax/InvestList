using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class CommentRepository(
        ApplicationDbContext dbContext,
        IUserRepository userRepository,
        IInvestAdRepository investAdRepository,
    INewsRepository newsRepository)
        : ICommentRepository
    {
        public async Task PublishAsync(Comment comment)
        {
            if (comment.InvestAdId.HasValue && comment.NewsId.HasValue)
                throw new ArgumentException("Comment can be either for News or for InvestAds");
            _ = await userRepository.Get(comment.UserId) ?? throw new NullReferenceException("User not found");
            
            if (comment.InvestAdId != null)
                _ = await investAdRepository.Get(comment.InvestAdId.Value) ??
                    throw new NullReferenceException("Ads not found");
            if (comment.NewsId != null)
                _ = await newsRepository.Get(comment.NewsId.Value) ??
                    throw new NullReferenceException("News not found");
            comment.CreatedAt = DateTimeOffset.UtcNow;
            comment.Id = Guid.NewGuid();
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }
    }
}