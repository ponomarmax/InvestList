using AutoMapper;
using Core;
using DataAccess.Models;
using DataAccess.Repositories.V2;
using InvestList.Models.V2;

namespace InvestList.Services
{
    public interface IInvestService
    {
        Task<string> Put(string? idString, string userId, PutInvestModel putInvestModel);
    }

    public class InvestService(IMapper mapper, IInvestRepository repository): IInvestService
    {
        public async Task<string> Put(string? idString, string userId, PutInvestModel putInvestModel)
        {
            putInvestModel.MinInvestValues = putInvestModel.MinInvestValues.Where(x => x.MinValue.HasValue);
            var invest = mapper.Map<InvestPost>(putInvestModel);
            var post = mapper.Map<Post>(putInvestModel);
            invest.Post = post;
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
                post.Slug = await CreateSlug(post.Title);
                post.PostType = PostType.InvestAd;
                await repository.Create(invest);
            }

            return idString ?? post.Slug;
        }

        private async Task<string> CreateSlug(string title)
        {
            var slug = SlugGenerator.Get(title);
            if (await repository.Exists(slug))
                return $"{slug}-{Guid.NewGuid().ToString()[..7]}";
            return slug;
        }
    }
}