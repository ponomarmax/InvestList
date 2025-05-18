using Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.Infrastructure.Authorization;

namespace InvestList.Areas.Main.Pages.Subscription
{
    [RequireConfirmedEmail]
    public class Checkout(IUserRepository repository): PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await repository.IssueWeekSubscription(Utils.GetUserId(User));
            return Page();
        }
    }
}