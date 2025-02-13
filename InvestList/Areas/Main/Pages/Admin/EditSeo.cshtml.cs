using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Entities;
using InvestList.Services;
using Microsoft.AspNetCore.Authorization;
using Radar.Domain.Entities;

namespace InvestList.Areas.Main.Pages.Admin;

[Authorize(Roles = Const.AdminRole)]
public class EditSeo(ISeoService seoService) : PageModel
{
    [BindProperty]
    public SeoDetails SeoDetails { get; set; }

    public async Task<IActionResult> OnGetAsync(string pagePath = null)
    {
        if (string.IsNullOrEmpty(pagePath))
        {
            return Page();
        }

        SeoDetails = await seoService.GetSeoDetailsAsync(pagePath);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await seoService.SaveSeoDetailsAsync(SeoDetails);

        return RedirectToPage("./AllSeo");
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await seoService.DeleteSeoDetailsAsync(id);
        return RedirectToPage("./AllSeo");
    }

}