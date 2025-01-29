using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestList.Areas.Main.Pages.Blacklist;

public class Get(IPostRepository repository, IMapper mapper, UserManager<User> userManager): PageModel
{
    public bool CanUserEdit { get; set; }
    public PostView Post { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (Guid.TryParse(id, out _))
        {
            var dbModel = await repository.Get(id);
            if (dbModel == null)
            {
                return NotFound();
            }

            return RedirectToPagePermanent("./Get", new { id = dbModel.Slug });
        }

        if (string.IsNullOrEmpty(id)) return NotFound();

        var post = await repository.Get(id, PostType.Blacklist);
        if (post == null) return NotFound();

        CanUserEdit = await userManager.CanEditPost(User);
        
        Post = mapper.Map<PostView>(post);

        var tagIds = Post.Tags.Select(x => x.Id).ToList();

        var similarContent = (await repository.GetSimilarPosts(post.Id, tagIds)).ToList();
        
        Post.SimilarNews = mapper.Map<IEnumerable<PostView>>(similarContent.Where(x=>x.PostType==PostType.News));
        Post.SimilarInvests = mapper.Map<IEnumerable<PostView>>(similarContent.Where(x=>x.PostType==PostType.InvestAd));
        ViewData.SetupPostViewSeoDetails(Post);

        return Page();
    }
}