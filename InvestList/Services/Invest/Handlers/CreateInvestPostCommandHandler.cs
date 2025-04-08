using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Services.Invest.Commands;
using MediatR;
using Radar.Application;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;

namespace InvestList.Services.Invest.Handlers;

public class CreateInvestPostCommandHandler(
    IInvestRepository repository,
    IImageService imageService,
    IPostFactoryService postFactory,
    IMapper mapper)
    : IRequestHandler<CreateInvestPostCommand, string>
{
    public async Task<string> Handle(CreateInvestPostCommand command, CancellationToken ct)
    {
        command.Post.PostType = PostType.InvestAd.ToString();

        var post = await postFactory.CreateAsync(command.Post, command.UserId, ct);

        var investPost = mapper.Map<InvestPost>(command.InvestPost);
        investPost.Post = post;
        
        var slug = await repository.Create(investPost);
        imageService.RefreshImages(post, null);

        return slug;
    }
}