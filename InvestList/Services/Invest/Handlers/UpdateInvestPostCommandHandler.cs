using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Services.Invest.Commands;
using MediatR;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;

namespace InvestList.Services.Invest.Handlers;

public class UpdateInvestPostCommandHandler(
    IInvestRepository repository,
    IImageService imageService,
    IMapper mapper) : IRequestHandler<UpdateInvestPostCommand, string>
{
    // private readonly IPostCacheService _cache;
    // private readonly IPostListCacheService _listCache;
    
    public async Task<string> Handle(UpdateInvestPostCommand command, CancellationToken ct)
    {
        var post = mapper.Map<Post>(command.Post, opt =>
        {
            opt.Items["UpdatedById"] = command.UserId;
        });         var investPost = mapper.Map<InvestPost>(command.InvestPost);
        investPost.Post = post;
        var slug = await repository.Put(command.Id, investPost);
        imageService.RefreshImages(investPost.Post, null);

        return slug;
    }
}