using Common;
using InvestList.Services;
using InvestList.Services.Invest.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Domain.Entities;
using Radar.Infrastructure.Authorization;

namespace InvestList.Areas.Main.Pages.Invest
{
    [RequireConfirmedEmail]
    public class Create(
        IMediator mediator,
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
            BasePost();

            if (!ModelState.IsValid)
            {
                await PrepareTags();
                return Page();
            }
            
            var command = new CreateInvestPostCommand
            {
                Post = Post,
                InvestPost = InvestPost,
                UserId = Utils.GetUserId(User),
            };

            var slug = await mediator.Send(command);
            
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}