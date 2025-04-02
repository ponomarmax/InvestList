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
        Task<string> Put(string? idString, string userId, PostDataDto post, InvestPostDto investPostDto);
        Task<int> Count(PostType type = PostType.InvestAd);
    }

    public class InvestService(IMapper mapper, IServiceProvider serviceProvider): IInvestService
    {
        private int? _investCount;

        public async Task<string> Put(string? idString, string userId, PostDataDto post,  InvestPostDto investPostDto)
        {
            investPostDto.MinInvestValues = investPostDto.MinInvestValues.Where(x => x.MinValue.HasValue).ToList();
            
            var invest = mapper.Map<InvestPost>(investPostDto);
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