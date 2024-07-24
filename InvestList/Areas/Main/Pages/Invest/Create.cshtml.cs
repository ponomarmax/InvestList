using Common;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvestList.Areas.Main.Pages.Invest
{
    public class Create(
        IInvestService service,
        ITagRepository tagRepository,
        UserManager<User> userManager): PageModel
    {
        [BindProperty]
        public PutInvestModel Post { get; set; }

        public List<SelectListItem> AvailableTags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

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

            if (!ModelState.IsValid)
            {
                await PrepareViewData();
                return Page();
            }

            Post.Description = RemoveHtmlTags(Post.Description);

            var slug = await service.Put(null, Utils.GetUserId(User), Post);
            return RedirectToPage("./Get", new { id = slug });
        }
        
        private async Task PrepareViewData()
        {
            var tagsV2 = await tagRepository.GetTagsV2();
            foreach (var tag in tagsV2)
            {
                var item = new SelectListItem(tag.Name, tag.Id.ToString());
                AvailableTags.Add(item);
            }
        }

        private string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = Regex.Replace(input, @"</?div.*?>", "\n", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<br>", "\n", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<b>", "**", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"</b>", "**", RegexOptions.IgnoreCase);
            return input.Trim();
        }

        public string AddHtmlTags(string input) 
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = Regex.Replace(input, @"\*\*", "<b>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\*\*", "</b>", RegexOptions.IgnoreCase);
            return input.Trim();
        }
    }
}