using AutoMapper;
using Core;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;

namespace InvestList.Services
{
    public interface IInvestService
    {
        Task<string> Put(string? idString, string userId, PutInvestModel putInvestModel);
        Task<int> Count(PostType type = PostType.InvestAd);
    }

    public class InvestService(IMapper mapper, IServiceProvider serviceProvider): IInvestService
    {
        private int? _investCount;

        public async Task<string> Put(string? idString, string userId, PutInvestModel putInvestModel)
        {
            putInvestModel.MinInvestValues = putInvestModel.MinInvestValues.Where(x => x.MinValue.HasValue);
            var invest = mapper.Map<InvestPost>(putInvestModel);
            var post = mapper.Map<Post>(putInvestModel);
            invest.Post = post;
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<IInvestRepository>();
            if (Guid.TryParse(idString, out var id))
            {
                post.Id = id;
                post.UpdatedAt = DateTime.UtcNow;
                await repository.Put(id, invest);
            }
            else
            {
                post.Id = Guid.NewGuid();
                post.CreatedAt = DateTime.UtcNow;
                post.CreatedById = userId;
                post.UpdatedAt = post.CreatedAt;
                post.Slug = await CreateSlug(post.Title, repository);
                post.PostType = PostType.InvestAd.ToString();
                await repository.Create(invest);
                _investCount = null;
            }

            return idString ?? post.Slug;
        }
        
        public async Task<int> Count(PostType type = PostType.InvestAd)
        {
            if (_investCount == null)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetService<IPostRepository>();
                _investCount = await repository.Count(type);
            }
            
            return _investCount.Value;
        }

        private async Task<string> CreateSlug(string? title, IInvestRepository repository)
        {
            var slug = SlugGenerator.Get(title);
            if (await repository.Exists(slug))
                return $"{slug}-{Guid.NewGuid().ToString()[..7]}";
            return slug;
        }
    }
}