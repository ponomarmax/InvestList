using AutoMapper;
using Common;
using Core.Entities;
using Core.Interfaces;
using InvestList.Filters;
using InvestList.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestList.Controllers
{
    [Authorize]
    public class CommentController(ICommentRepository commentRepository, IMapper mapper): Controller
    {
        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Publish(PostCommentRequest request)
        {
            request.UserId = Guid.Parse(Utils.GetUserId(User));
            var db = mapper.Map<PostComment>(request);
            await commentRepository.PublishAsync(db);
            return request.PostType switch
            {
                PostType.InvestAd => RedirectToPagePermanent("/Invest/Get", new { area = "Main", id = request.PostId }),
                PostType.Blacklist => RedirectToPagePermanent("/Blacklist/Get", new { area = "Main", id = request.PostId }),
                _ => RedirectToPagePermanent("/News/Get", new { area = "Main", id = request.PostId })
            };
        }
    }
}