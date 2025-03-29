using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application.Models;
using Radar.Domain.Entities;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.News;

public class Get(IPostRepository repository, IMapper mapper, UserManager<User> userManager): BaseGetPage
{
    public async Task<IActionResult> OnGetAsync(string slug)
    {
        if (string.IsNullOrEmpty(slug)) return NotFound();

        var post = await repository.Get(slug, PostType.News);
        if (post == null) return NotFound();

        CanUserEdit = await userManager.CanEditPost(User);
        
        Post = mapper.Map<PostView>(post);

        var tagIds = Post.Tags.Select(x => x.Id).ToList();
    
        var similarContent = (await repository.GetSimilarPosts(post.Id, tagIds)).ToList();
        
        Post.SimilarNews = mapper.Map<IEnumerable<PostView>>(similarContent.Where(x=>x.PostType==PostType.News.ToString()));
        Post.SimilarInvests = mapper.Map<IEnumerable<PostView>>(similarContent.Where(x=>x.PostType==PostType.InvestAd.ToString()));
        Radar.UI.SeoHelper.SetupPostViewSeoDetails(ViewData, Post);

        return Page();
    }
}