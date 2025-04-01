using AutoMapper;
using Common;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Application.Models;
using Radar.Domain.Entities;
using Radar.EF.Authorization;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Invest
{
    [IsPostOwnerAuthorize]
    public class Edit(IInvestRepository repository, IInvestService service, ITagService tagService, IMapper mapper,ISanitizerService sanitizerService) :  BaseInvestUpsertPage(tagService,sanitizerService)
    {
        public Guid Id { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            var postFormModel = mapper.Map<PostFormModel>(db.Post);
            InvestPost = mapper.Map<PutInvestModel>(db);
            Prepare();
            await PrepareTags(postFormModel);
            Id = db.Post.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                Id = id;
                await PrepareTags(Post);
                return Page();
            }
            
            BasePost();
            
            var db = await repository.Get(id.ToString());
            if (db == null)
                return NotFound();

            var slug = await service.Put(id.ToString(), Utils.GetUserId(User), Post, InvestPost);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}