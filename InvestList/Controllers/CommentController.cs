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
            var db = mapper.Map<Comment>(request);
            await commentRepository.PublishAsync(db);
            if (request.InvestAdId.HasValue)
                return RedirectToAction("Details", "Invest", new { id = request.InvestAdId });
            return RedirectToAction("Details", "News", new { id = request.NewsId });
        }
    }
}