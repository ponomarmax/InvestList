using AutoMapper;
using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Application.Models;
using Radar.Application.Posts.Commands;
using Radar.Domain.Interfaces;
using Radar.Infrastructure.Authorization;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Blacklist
{
    [IsAdminAuthorize]
    public class Edit(IBasePostRepository repository, ITagService tagService, IMapper mapper, ISanitizerService sanitizerService, IMediator mediator) : BaseUpsertPage(tagService, sanitizerService)
    {
        public Guid Id { get; set; }
        
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var db = await repository.Get(id.ToString());
            var postFormModel = mapper.Map<PostDataDto>(db);
            await PrepareTags(postFormModel);
            Id = db.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var db = await repository.Get(id.ToString());
            if (db == null)
                return NotFound();

            BasePost();
            var command = new UpdatePostCommand
            {
                Id = id,
                Post = Post,
            };


            var slug = await mediator.Send(command);
            return RedirectToPage("./Get", new { id = slug });
        }
    }
}