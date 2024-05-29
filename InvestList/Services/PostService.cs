using AutoMapper;
using Core;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;

namespace InvestList.Services
{
    public interface IPostService
    {
        Task<string> Put(string? idString, string userId, PutPostModel putInvestModel);
    }

    public class PostService(IMapper mapper, IPostRepository repository): IPostService
    {
        public async Task<string> Put(string? idString, string userId, PutPostModel putInvestModel)
        {
            var post = mapper.Map<Post>(putInvestModel);
            if (Guid.TryParse(idString, out var id))
            {
                post.Id = id;
                post.UpdatedAt = DateTime.UtcNow;
                await repository.Put(id, post);
            }
            else
            {
                post.Id = Guid.NewGuid();
                post.CreatedAt = DateTime.UtcNow;
                post.CreatedById = userId;
                post.UpdatedAt = post.CreatedAt;
                post.Slug = await CreateSlug(post.Title);
                post.PostType = PostType.News;
                await repository.Create(post);
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