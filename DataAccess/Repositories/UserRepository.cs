using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radar.Domain.Entities;
using Radar.Infrastructure.Repositories;

namespace DataAccess.Repositories
{
    public class UserRepository(ApplicationDbContext dbContext, UserManager<User> userManager): BaseUserRepository(userManager, dbContext), IUserRepository
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
        
        public async Task IssueWeekSubscription(string userId, CancellationToken cancellationToken = default)
        {
            await dbContext.Users
                .Where(u => u.Id==userId) // Example condition
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.SubscriptionExpiresOn, DateTime.UtcNow.AddDays(7)));
        }
        
        public async Task<User?> Get(string id) => await dbContext.Users.FindAsync(id);
    }
}
