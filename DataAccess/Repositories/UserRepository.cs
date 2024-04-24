using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsEmailConfirmed(string userId)
        {
            return await _dbContext.Users.AnyAsync(x => x.Id == userId && x.EmailConfirmed);
        }

        public async Task<User?> Get(string id) => await _dbContext.Users.FindAsync(id);
    }
}
