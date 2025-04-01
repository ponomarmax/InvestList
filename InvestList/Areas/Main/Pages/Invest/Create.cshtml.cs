using Common;
using InvestList.Models.V2;
using InvestList.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Radar.Application;
using Radar.Domain.Entities;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Invest
{
    public class Create(
        IInvestService service,
        ITagService tagService,
        UserManager<User> userManager,ISanitizerService sanitizerService):  BaseInvestUpsertPage(tagService,sanitizerService)
    {
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();
            Prepare();
            await PrepareTags();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();
            
            BasePost();

            // if (!await userManager.IsEmailConfirmedAsync(user))
            //     return RedirectToPage("/Account/ResendEmailConfirmation", new { area = "Identity" });

            if (!ModelState.IsValid)
            {
                await PrepareTags();
                return Page();
            }
            var slug = await service.Put(null, Utils.GetUserId(User), Post, InvestPost);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}