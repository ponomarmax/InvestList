using AutoMapper;
using Common;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Application.Models;
using Radar.Domain.Entities;
using Radar.EF.Authorization;
using Radar.UI.Models;
using IPostService = InvestList.Services.IPostService;

namespace InvestList.Areas.Main.Pages.News
{
    [IsAdminAuthorize]
    public class Edit(IPostRepository repository, IPostService service, ITagService tagService, UserManager<User> userManager, IMapper mapper) : BaseUpsertPage(tagService)
    {
        public Guid Id { get; set; }
        
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            var postFormModel = mapper.Map<PostFormModel>(db);
            await PrepareTags(postFormModel);
            Id = db.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            if (db == null)
                return NotFound();
            
            if (!ModelState.IsValid)
            {
                var postFormModel = mapper.Map<PostFormModel>(db);
                await PrepareTags(postFormModel);
                return Page();
            }

            var post = mapper.Map<Post>(Post);
            var slug = await service.Put(id.ToString(), Utils.GetUserId(User),  post);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}