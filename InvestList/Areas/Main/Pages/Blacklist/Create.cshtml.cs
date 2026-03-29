using Common;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Radar.Application;
using Radar.Application.Posts.Commands;
using Radar.Infrastructure.Authorization;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Blacklist
{
    [IsAdminAuthorize]
    public class Create(
        ITagService tagService,
        ISanitizerService sanitizerService,
        IMediator mediator): BaseUpsertPage(tagService, sanitizerService)
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await PrepareTags();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!await userManager.IsEmailConfirmedAsync(user))
            //     return RedirectToPage("/Account/ResendEmailConfirmation", new { area = "Identity" });
            
            // if (!ModelState.IsValid)
            // {
            //     await PrepareTags();
            //     return Page();
            // }
            BasePost();
            var command = new CreatePostCommand
            {
                UserId = Utils.GetUserId(User),
                Post = Post
            };

            // Set the post type
            Post.PostType = PostType.Blacklist.ToString();

            var slug = await mediator.Send(command);

            return RedirectToPage("./Get", new { id = slug });
        }
    }
}