using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Entities;
using InvestList.Services;
using Microsoft.AspNetCore.Authorization;

namespace InvestList.Areas.Main.Pages.Admin;

[Authorize(Roles = Const.AdminRole)]
public class AllSeoModel(ISeoService seoService) : PageModel
{
    public List<SeoDetails> SeoDetailsList { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        SeoDetailsList = await seoService.GetAllSeoDetailsAsync();
        return Page();
    }

    public IActionResult OnGetCreateNewSeo()
    {
        return RedirectToPage("/EditSeo");
    }
}