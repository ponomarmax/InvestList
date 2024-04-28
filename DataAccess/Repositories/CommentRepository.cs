using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class CommentRepository: ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IInvestAdRepository _investAdRepository;

        public CommentRepository(ApplicationDbContext dbContext, IUserRepository userRepository, IInvestAdRepository investAdRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _investAdRepository = investAdRepository;
        }

        public async Task PublishAsync(Comment comment)
        {
            _ = await _userRepository.Get(comment.UserId) ?? throw new NullReferenceException("User not found");
            _ = await _investAdRepository.Get(comment.InvestAdId)?? throw new NullReferenceException("Ads not found");
            comment.CreatedAt = DateTimeOffset.UtcNow;
            comment.Id = Guid.NewGuid();
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}