using AutoMapper;
using Common;
using Core.Interfaces;
using InvestList.Models.V2;
using InvestList.Services;
using InvestList.Services.Invest.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Application.Models;
using Radar.Domain.Entities;
using Radar.Infrastructure.Authorization;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Invest
{
    [IsPostOwnerAuthorize]
    public class Edit(IInvestRepository repository, IMediator mediator, ITagService tagService, IMapper mapper,ISanitizerService sanitizerService) :  BaseInvestUpsertPage(tagService,sanitizerService)
    {
        public Guid Id { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            var postFormModel = mapper.Map<PostDataDto>(db.Post);
            InvestPost = mapper.Map<InvestPostDto>(db);
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
            var command = new UpdateInvestPostCommand
            {
                Id = id,
                Post = Post,
                InvestPost = InvestPost
            };


            var slug = await mediator.Send(command);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}