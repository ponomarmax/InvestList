using System.Globalization;
using AutoMapper;
using Core;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;
using Radar.Application.Models;

namespace InvestList.Services
{
    public interface IInvestService
    {
        Task<string> Put(string? idString, string userId, PostFormModel post, PutInvestModel putInvestModel);
        Task<int> Count(PostType type = PostType.InvestAd);
    }

    public class InvestService(IMapper mapper, IServiceProvider serviceProvider): IInvestService
    {
        private int? _investCount;

        public async Task<string> Put(string? idString, string userId, PostFormModel post,  PutInvestModel putInvestModel)
        {
            putInvestModel.MinInvestValues = putInvestModel.MinInvestValues.Where(x => x.MinValue.HasValue).ToList();
            
            var invest = mapper.Map<InvestPost>(putInvestModel);
            var postDb = mapper.Map<Post>(post);
            invest.Post = postDb;
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<IInvestRepository>();
            var imageService = scope.ServiceProvider.GetService<IImageService>();
            Guid[] oldImagePaths = null;
            if (Guid.TryParse(idString, out var id))
            {
                await repository.Put(id, invest);
                 oldImagePaths = invest.Post.Images.Select(x => x.Id).ToArray();
            }
            else
            {
                var properTitle = postDb.Translations
                    .FirstOrDefault(x => x.Language == CultureInfo.CurrentCulture.ToString())?.Title??postDb.Translations
                    .FirstOrDefault()
                    ?.Title;
              
                postDb.CreatedById = userId;
                postDb.Slug = await CreateSlug(properTitle, repository);
                postDb.PostType = PostType.InvestAd.ToString();
                await repository.Put(null, invest);
                _investCount = null;
            }
            imageService.RefreshImages(postDb, oldImagePaths);

            // var investDb = await dbContext.InvestPosts.FindAsync(id);
            // var postOrigin = await Get(id);
            // 
            // if (postOrigin != null)
            // {
            //     mapper.Map(invest, postOrigin);
            //     
            // }
            // else
            // {
            //     throw new ArgumentException("Attempt to modify unexisting object");
            // }
            // postOrigin = await Get(id);
            // imageService.RefreshImages(postOrigin.Post, oldImagePaths);
            return idString ?? postDb.Slug;
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