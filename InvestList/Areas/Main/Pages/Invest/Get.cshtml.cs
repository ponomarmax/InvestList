using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.V2;
using InvestList.Models.V2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestList.Areas.Main.Pages.Invest;

public class Get(IInvestRepository repository, INewsRepository newsRepository, IMapper mapper, UserManager<User> userManager): PageModel
{
    public bool CanUserEdit { get; set; }
    public InvestView Post { get; set; }

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
        
        Post = mapper.Map<InvestView>(investPost);

        var tagIds = Post.Tags.Select(x => x.Id).ToList();

        var similarContent = (await repository.GetSimilarInvests(investPost.PostId, tagIds)
            ).Where(x=>x.PostType==PostType.InvestAd).Take(5);
        
        Post.SimilarNews = mapper.Map<IEnumerable<PostView>>(await newsRepository.GetSimilarNews(tagIds));
        Post.SimilarInvests = mapper.Map<IEnumerable<PostView>>(similarContent);
        ViewData.SetupPostViewSeoDetails(Post);

        return Page();
    }
}