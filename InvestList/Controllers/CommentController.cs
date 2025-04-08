using AutoMapper;
using Common;
using Core.Entities;
using Core.Interfaces;
using InvestList.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radar.Application.Models;
using Radar.Domain.Entities;

namespace InvestList.Controllers
{
    [Authorize]
    public class CommentController(ICommentRepository commentRepository, IMapper mapper): Controller
    {
        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Publish(PostCommentRequest request)
        {
            request.UserId = Guid.Parse(Utils.GetUserId(User));
            var db = mapper.Map<PostComment>(request);
            await commentRepository.PublishAsync(db);
            Enum.TryParse<PostType>(request.PostType, ignoreCase: true, out var postType);
            return postType  switch
            {
                PostType.InvestAd => RedirectToPagePermanent("/Invest/Get", new { area = "Main", id = request.PostId }),
                PostType.Blacklist => RedirectToPagePermanent("/Blacklist/Get", new { area = "Main", id = request.PostId }),
                PostType.News => RedirectToPagePermanent("/News/Get", new { area = "Main", id = request.PostId }),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}