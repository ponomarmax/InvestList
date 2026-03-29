using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Radar.Domain.Interfaces;

namespace InvestList.Services
{
    public interface IInvestService
    {
        Task<int> Count(PostType type = PostType.InvestAd);
    }

    public class InvestService( IServiceProvider serviceProvider): IInvestService
    {
        private int? _investCount;

        
        public async Task<int> Count(PostType type = PostType.InvestAd)
        {
            if (_investCount == null)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetService<IBasePostRepository>();
                _investCount = await repository.Count(type.ToString());
            }
            
            return _investCount.Value;
        }
    }
}