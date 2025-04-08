using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Services.Queries;
using MediatR;
using Radar.Application;
using Radar.Application.Models;
using Radar.Domain.Interfaces;

namespace InvestList.Services.Handlers;

public class GetPrimarySectionsHandler(
    IInvestRepository repository,
    IMapper mapper,
    IImagePathBuilder imagePathBuilder,
    ISlugResolver slugResolver)
    : IRequestHandler<GetPrimarySectionsQuery, PrimarySectionsDto>
{
    public async Task<PrimarySectionsDto> Handle(GetPrimarySectionsQuery request, CancellationToken cancellationToken)
    {
        var similarPosts =
            await repository.GetGroupedPostsWithInvestAsync(request.Language, request.Search, request.TagIds,
                cancellationToken);

        var comments = await repository.GetPostsWithLastComments(request.Language);

        var re = new PrimarySectionsDto();
        if (similarPosts.TryGetValue(PostType.Blacklist.ToString(), out var similarPostBlacklist))
        {
            re.Blacklists =
                mapper.Map<List<PostShortDto>>(similarPostBlacklist.Select(x => x.Post),
                    opt =>
                    {
                        opt.Items["Language"] = request.Language;
                        opt.Items["ImagePathBuilder"] = imagePathBuilder;
                    });
        }

        if (similarPosts.TryGetValue(PostType.News.ToString(), out var similarPostNews))
        {
            re.News =
                mapper.Map<List<PostShortDto>>(similarPostNews.Select(x => x.Post),
                    opt =>
                    {
                        opt.Items["Language"] = request.Language;
                        opt.Items["ImagePathBuilder"] = imagePathBuilder;
                    });
        }

        if (similarPosts.TryGetValue(PostType.InvestAd.ToString(), out var similarPostInvests))
        {
            re.Invests =
                mapper.Map<List<InvestPostShortDto>>(similarPostInvests.Select(x =>
                    {
                        x.Invest.Post = x.Post;
                        return x.Invest;
                    }),
                    opt =>
                    {
                        opt.Items["Language"] = request.Language;
                        opt.Items["ImagePathBuilder"] = imagePathBuilder;
                    });
        }

        re.PostWithComments = mapper.Map<List<PostShortWithCommentDto>>(comments,
            opt =>
            {
                opt.Items["Language"] = request.Language;
                opt.Items["ImagePathBuilder"] = imagePathBuilder;
                opt.Items["SlugResolver"] = slugResolver;
            });

        return re;
    }
}