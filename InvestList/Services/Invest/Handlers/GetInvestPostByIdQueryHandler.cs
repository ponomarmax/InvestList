using AutoMapper;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services.Invest.Queries;
using MediatR;
using Radar.Application;
using Radar.Application.Models;
using Radar.Application.Posts.Queries;
using Radar.Domain.Interfaces;

namespace InvestList.Services.Invest.Handlers;

public class GetInvestPostByIdQueryHandler(IInvestRepository repository, IMapper mapper, IImagePathBuilder imagePathBuilder)
    : IRequestHandler<GetInvestPostByIdQuery, InvestPostDetailDto?>
{
    // private readonly IPostCacheService _cache;

    // IPostCacheService cache,
    // _cache = cache;

    public async Task<InvestPostDetailDto?> Handle(GetInvestPostByIdQuery query, CancellationToken cancellationToken)
    {
        // var cached = await _cache.GetAsync(query.Id);
        // if (cached is not null)
        //     return cached;

        var post = await repository.Get(query.Slug);
        if (post is null) return null;

        var dto = mapper.Map<InvestPostDetailDto>(post, opt =>
        {
            opt.Items["Language"] = query.Language;
            opt.Items["ImagePathBuilder"] = imagePathBuilder;
        });
        // await _cache.SetAsync(query.Id, dto);
        return dto;
    }
}