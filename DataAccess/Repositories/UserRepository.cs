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

        public async Task<User?> Get(string id) => await dbContext.Users.FindAsync(id);
    }
}
