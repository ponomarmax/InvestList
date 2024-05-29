using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailConfirmed(string userId);
        Task<User?> Get(string id);
    }
}
