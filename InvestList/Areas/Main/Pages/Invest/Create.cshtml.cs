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
    [IsPostOwnerAuthorize]
    public class Create(
        IMediator _mediator,
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
            BasePost();

            // if (!await userManager.IsEmailConfirmedAsync(user))
            //     return RedirectToPage("/Account/ResendEmailConfirmation", new { area = "Identity" });

            if (!ModelState.IsValid)
            {
                await PrepareTags();
                return Page();
            }
            
            var command = new CreateInvestPostCommand
            {
                UserId = user.Id,
                Post = Post,
                MinInvestValues = InvestPostPost.MinInvestValues,
                InvestDurationYears = InvestPostPost.InvestDurationYears,
                InvestDurationMonths = InvestPostPost.InvestDurationMonths,
                TotalInvestment = InvestPostPost.TotalInvestment,
                AnnualInvestmentReturn = InvestPostPost.AnnualInvestmentReturn
            };

            var id = await _mediator.Send(command);
            
            var slug = await service.Put(null, Utils.GetUserId(User), Post, InvestPostPost);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}