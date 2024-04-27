using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailConfirmed(string userId);
        Task<User?> Get(string id);
    }
}
