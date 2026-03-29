using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Services.Invest.Queries;
using InvestList.Services.Queries;
using MediatR;
using Radar.Application.Models;
using Radar.Domain;
using Radar.Domain.Interfaces;

namespace InvestList.Services.Invest.Handlers;

public class GetInvestPostListQueryHandler(
    IInvestRepository repository,
    IMapper mapper,
    IImagePathBuilder imagePathBuilder)
    : IRequestHandler<GetInvestPostListQuery, PaginatedResult<InvestPostShortDto>>
{
    // private readonly IPostListCacheService _cache;

    // IPostListCacheService cache,
    // _cache = cache;

    public async Task<PaginatedResult<InvestPostShortDto>> Handle(GetInvestPostListQuery query,
        CancellationToken cancellationToken)
    {
        // var isCacheable = string.IsNullOrWhiteSpace(query.Search) && query.TagIds.Count == 0 && query.Page == 1;
        // var cacheKey = $"posts:page={query.Page}&size={query.PageSize}&search={query.Search}&tags={string.Join(",", query.TagIds)}";
        //
        // if (isCacheable)
        // {
        //     var cached = await _cache.GetAsync(cacheKey);
        //     if (cached is not null)
        //         return cached;
        // }

        var request = new PaginationData()
        {
            Page = query.Page,
            Language = query.Language,
            PostType = PostType.InvestAd.ToString(),
            Search = query.Search,
            TagsIds = query.TagIds,
            Take = query.PageSize
        };
        var result = await repository.Filter(request);
        var dtos = mapper.Map<List<InvestPostShortDto>>(result.list, opt =>
        {
            opt.Items["Language"] = query.Language;
            opt.Items["ImagePathBuilder"] = imagePathBuilder;
        });

        var paged = new PaginatedResult<InvestPostShortDto>
        {
            Items = dtos,
            TotalCount = result.count,
            Page = query.Page,
            PageSize = query.PageSize
        };

        // if (isCacheable)
        //     await _cache.SetAsync(cacheKey, paged);

        return paged;
    }
}