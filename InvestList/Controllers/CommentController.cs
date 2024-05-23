using AutoMapper;
using Common;
using DataAccess.Interfaces;
using DataAccess.Models;
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
            if (request.InvestAdId.HasValue)
            {
                var db = mapper.Map<PostComment>(request);
                await commentRepository.PublishAsync(db);
                return RedirectToPagePermanent("/Invest/Get", new { area="Main", id = request.InvestAdId });
            }
            var db1 = mapper.Map<Comment>(request);
            await commentRepository.PublishAsync(db1);
            return RedirectToAction("Details", "News", new { id = request.NewsId });
        }
    }
}