using AutoMapper;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services.Invest.Queries;
using MediatR;
using Radar.Application;
using Radar.Application.Models;

namespace InvestList.Services.Invest.Handlers;

public class GetInvestPostWithSimilarQueryHandler(
    IInvestRepository postRepository,
    IMapper mapper,
    IImagePathBuilder imagePathBuilder)
    : IRequestHandler<GetInvestPostWithSimilarQuery, InvestPostWithSimilarResponseDto>
{
    public async Task<InvestPostWithSimilarResponseDto> Handle(GetInvestPostWithSimilarQuery query, CancellationToken cancellationToken)
    {
        // Step 1: Load the main post
        var mainPost = await postRepository.Get(query.Slug);
           
        if (mainPost == null)
        {
            return new InvestPostWithSimilarResponseDto
            {
                InvestPost = null,
                SimilarPosts = new Dictionary<string, List<PostDetailDto>>()
            };
        }

        var mainTagIds = mainPost.Post.Tags.Select(t => t.TagId).ToList();

        var similarPosts = await postRepository.GetSimilarPosts(mainPost.Id, mainTagIds, 4);
        var grouped = similarPosts
            .GroupBy(p => p.PostType)
            .ToDictionary(
                g => g.Key,
                g => g
                    .Take(10)
                    .Select(p => mapper.Map<PostDetailDto>(p, opt =>
                    {
                        opt.Items["Language"] = query.Language;
                        opt.Items["ImagePathBuilder"] = imagePathBuilder;
                    }))
                    .ToList()
            );

        return new InvestPostWithSimilarResponseDto
        {
            InvestPost = mapper.Map<InvestPostDetailDto>(mainPost, opt =>
            {
                opt.Items["Language"] = query.Language;
                opt.Items["ImagePathBuilder"] = imagePathBuilder;
            }),
            SimilarPosts = grouped
        };
    }
}