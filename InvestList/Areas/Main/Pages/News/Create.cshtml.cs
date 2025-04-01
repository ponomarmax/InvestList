using AutoMapper;
using Common;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Domain.Entities;
using Radar.EF.Authorization;
using Radar.UI.Models;
using IPostService = InvestList.Services.IPostService;

namespace InvestList.Areas.Main.Pages.News
{
    [IsAdminAuthorize]
    public class Create(
        IPostService service,
        ITagService tagService,
        ISanitizerService sanitizerService,
        IMapper mapper): BaseUpsertPage(tagService, sanitizerService)
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await PrepareTags();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
                // if (!await userManager.IsEmailConfirmedAsync(user))
                //     return RedirectToPage("/Account/ResendEmailConfirmation", new { area = "Identity" });
            
            if (!ModelState.IsValid)
            {
                await PrepareTags();
                return Page();
            }
            BasePost();
            var post = mapper.Map<Post>(Post);
            var slug = await service.Put((string?)null, Utils.GetUserId(User), post, PostType.News);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}