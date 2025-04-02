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

        public async Task<User?> Get(string id) => await dbContext.Users.FindAsync(id);
    }
}
