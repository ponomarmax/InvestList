using AutoMapper;
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
    // [Authorize(Roles = B)]
    // [EmailConfirmedAuthorize]
    public class Edit(IInvestRepository repository, IInvestService service, ITagRepository tagRepository, UserManager<User> userManager, IMapper mapper) : PageModel
    {
        public Guid Id { get; set; }
        
        [BindProperty]
        public PutInvestModel Post { get; set; }
        
        public List<SelectListItem> AvailableTags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var db = await repository.Get(id);
            if (db == null)
                return NotFound();

            if (!await userManager.CanEditInvestPost(User, db))
            {
                return Forbid();
            }

            await PrepareViewData(id, db);

            // Post.Description = AddHtmlTags(Post.Description);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var db = await repository.Get(id);
            if (db == null)
                return NotFound();
           
            if (!await userManager.CanEditInvestPost(User, db))
            {
                return Forbid();
            }
            
            if (!ModelState.IsValid)
            {
                await PrepareViewData(id, db);
                return Page();
            }

            // Post.Description = RemoveHtmlTags(Post.Description);

            var slug = await service.Put(id.ToString(), Utils.GetUserId(User),  Post);
            return RedirectToPage("./Get", new { id = slug });
        }
        
        private async Task PrepareViewData(Guid id, InvestPost db)
        {
            Id = id;
            var tagsV2 = await tagRepository.GetTagsV2();
            Post = mapper.Map<PutInvestModel>(db);
            foreach (var tag in tagsV2)
            {
                var item = new SelectListItem(tag.Name, tag.Id.ToString());
                if (db.Post.Tags.FirstOrDefault(x => x.TagId == tag.Id) != null)
                    item.Selected = true;
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
            input = Regex.Replace(input, "<.*?>", string.Empty);
            return input.Trim();
        }

        public string AddHtmlTags(string input) 
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = Regex.Replace(input, @"\*\*(.*?)\*\*", "<b>$1</b>", RegexOptions.Singleline);
            return input.Trim();
        }
    }
}