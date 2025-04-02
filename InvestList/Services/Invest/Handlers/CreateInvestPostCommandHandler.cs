using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Services.Invest.Commands;
using MediatR;
using Radar.Application;

namespace InvestList.Services.Invest.Handlers;

public class CreateInvestPostCommandHandler : IRequestHandler<CreateInvestPostCommand, Guid>
{
    private readonly IInvestRepository _repository;
    private readonly IImageService _imageService;
    private readonly IPostFactoryService _postFactory;
    private readonly IMapper _mapper;

    public CreateInvestPostCommandHandler(
        IInvestRepository repository,
        IImageService imageService,
        IPostFactoryService postFactory,
        IMapper mapper)
    {
        _repository = repository;
        _imageService = imageService;
        _postFactory = postFactory;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateInvestPostCommand command, CancellationToken ct)
    {
        var post = await _postFactory.CreateAsync(command.Post, command.UserId, ct);
        var investPost = _mapper.Map<InvestPost>(command);
        investPost.Post = post;

        await _repository.CreateAsync(investPost);
        _imageService.RefreshImages(post);

        return investPost.Id;
    }
}