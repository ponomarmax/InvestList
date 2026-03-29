using System.Globalization;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services.Invest.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.Domain.Entities;

namespace InvestList.Areas.Main.Pages.Invest;

public class Get(IInvestRepository repository, IMediator mediator,  UserManager<User> userManager): PageModel
{
    public bool CanUserEdit { get; set; }
    public InvestPostDetailDto Post { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (Guid.TryParse(id, out var idGuid))
        {
            var dbModel = await repository.Get(idGuid);
            if (dbModel == null)
            {
                return NotFound();
            }

            return RedirectToPagePermanent("./Get", new { id = dbModel.Post.Slug });
        }

        if (string.IsNullOrEmpty(id)) return NotFound();

        var investPost = await repository.Get(id);
        if (investPost == null) return NotFound();

        CanUserEdit = await userManager.CanEditInvestPost(User, investPost);
        
        var postWithSimilar = await mediator.Send(new GetInvestPostWithSimilarQuery
        (
            id,
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
        ));
        
        Post = postWithSimilar.InvestPost;
        Post.Post.SimilarNews = postWithSimilar.SimilarPosts.TryGetValue(PostType.News.ToString(), out var similarNews)?similarNews:null; 
        Post.Post.SimilarInvests = postWithSimilar.SimilarPosts.TryGetValue(PostType.InvestAd.ToString(), out var similarAds)?similarAds:null;  
        Radar.UI.SeoHelper.SetupPostViewSeoDetails(ViewData, Post.Post);

        return Page();
    }
}