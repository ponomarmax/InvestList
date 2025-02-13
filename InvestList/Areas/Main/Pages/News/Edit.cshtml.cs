using AutoMapper;
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
    // [Authorize(Roles = B)]
    // [EmailConfirmedAuthorize]
    public class Edit(IPostRepository repository, IPostService service, ITagRepository tagRepository, UserManager<User> userManager, IMapper mapper) : PageModel
    {
        public Guid Id { get; set; }
        
        [BindProperty]
        public PutPostModel Post { get; set; }
        
        public List<SelectListItem> AvailableTags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            if (db == null)
                return NotFound();

            if (!await userManager.CanEditPost(User))
            {
                return Forbid();
            }

            await PrepareViewData(id, db);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            if (db == null)
                return NotFound();
           
            if (!await userManager.CanEditPost(User))
            {
                return Forbid();
            }
            
            if (!ModelState.IsValid)
            {
                await PrepareViewData(id, db);
                return Page();
            }

            var slug = await service.Put(id.ToString(), Utils.GetUserId(User),  Post);
            return RedirectToPage("./Get", new { id = slug });
        }
        
        private async Task PrepareViewData(Guid id, Post db)
        {
            Id = id;
            var tagsV2 = await tagRepository.GetTags();
            Post = mapper.Map<PutPostModel>(db);
            foreach (var tag in tagsV2)
            {
                var item = new SelectListItem(null, tag.Id.ToString());
                if (db.Tags.FirstOrDefault(x => x.TagId == tag.Id) != null)
                    item.Selected = true;
                AvailableTags.Add(item);
            }
        }
    }
}