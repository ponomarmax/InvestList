namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailConfirmed(string userId);
    }
}
