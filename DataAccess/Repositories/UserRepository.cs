using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository(ApplicationDbContext dbContext): IUserRepository
    {
        public async Task<bool> IsEmailConfirmed(string userId)
        {
            return await dbContext.Users.AnyAsync(x => x.Id == userId && x.EmailConfirmed);
        }
        
        public async Task SaveRequestInfo(UserRequestInfo requestInfo, CancellationToken cancellationToken = default)
        {
            await dbContext.UserRequestInfos.AddAsync(requestInfo, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        public async Task MarkAsFailedToSendEmail(string userId, CancellationToken cancellationToken = default)
        {
            await dbContext.Users
                .Where(u => u.Id==userId) // Example condition
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.FailedToSendEmailVerification, true));
        }
        
        public async Task<User?> Get(string id) => await dbContext.Users.FindAsync(id);
    }
}
