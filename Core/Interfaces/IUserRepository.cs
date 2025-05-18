using Core.Entities;
using Radar.Domain.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailConfirmed(string userId);
        Task<User?> Get(string id);
        Task SaveRequestInfo(UserRequestInfo requestInfo, CancellationToken cancellationToken = default);
        Task MarkAsFailedToSendEmail(string userId, CancellationToken cancellationToken = default);
        Task IssueWeekSubscription (string userId, CancellationToken cancellationToken = default);
    }
}
