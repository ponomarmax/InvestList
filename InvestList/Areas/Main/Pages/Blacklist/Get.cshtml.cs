using System.Globalization;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application.Posts.Queries;
using Radar.Domain.Entities;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Blacklist;

public class Get(IMediator mediator, UserManager<User> userManager): BaseGetPage
{
    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (string.IsNullOrEmpty(id)) return NotFound();

        CanUserEdit = await userManager.CanEditPost(User);
        var postWithSimilar = await mediator.Send(new GetPostWithSimilarQuery
        (
            id, PostType.Blacklist.ToString(),
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
        ));
        Post = postWithSimilar.Post;
        Post.SimilarNews = postWithSimilar.SimilarPosts.TryGetValue(PostType.News.ToString(), out var similarNews)?similarNews:null; 
        Post.SimilarInvests = postWithSimilar.SimilarPosts.TryGetValue(PostType.InvestAd.ToString(), out var similarAds)?similarAds:null;  
        Radar.UI.SeoHelper.SetupPostViewSeoDetails(ViewData, Post);

        return Page();
    }
}