using Common;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Radar.Domain.Entities;

namespace InvestList.Areas.Main.Pages.News
{
    public class Create(
        IPostService service,
        ITagRepository tagRepository,
        UserManager<User> userManager): PageModel
    {
        [BindProperty]
        public PutPostModel Post { get; set; }

        public List<SelectListItem> AvailableTags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            if (!await userManager.CanEditPost(User))
            {
                return Forbid();
            }
            await PrepareViewData();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            if (!await userManager.IsEmailConfirmedAsync(user))
                return RedirectToPage("/Account/ResendEmailConfirmation", new { area = "Identity" });
            
            if (!await userManager.CanEditPost(User))
            {
                return Forbid();
            }
            
            if (!ModelState.IsValid)
            {
                await PrepareViewData();
                return Page();
            }

            var slug = await service.Put(null, Utils.GetUserId(User), Post, PostType.News);
            return RedirectToPage("./Get", new { id = slug });
        }
        
        private async Task PrepareViewData()
        {
            var tagsV2 = await tagRepository.GetTags();
            foreach (var tag in tagsV2)
            {
                var item = new SelectListItem(null, tag.Id.ToString());
                AvailableTags.Add(item);
            }
        }
    }
}