using Core.Entities;
using InvestList.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.Application.Models;
using Radar.Application.Posts.Queries;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Subscription
{
    public class Checkout( IMediator mediator): PageModel
    {
        private const int ItemsPerPage = 50;

        public IEnumerable<PostShortDto> Entities { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public string Search { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, IEnumerable<string> tagIds = null, string search = null)
        {
            if (pageIndex < 1)
            {
                return NotFound();
            }

            Search = search;

            var guidTagIds = tagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse).ToList();

            var command = new GetPostListQuery()
            {
                Page = pageIndex,
                Search = search,
                TagIds = guidTagIds,
                PostType = PostType.News.ToString(),
                PageSize = ItemsPerPage
            };
        
            var result = await mediator.Send(command);
            if (!result.Items.Any() && pageIndex != 1) return NotFound();


            // ViewData.SetupListPostViewSeoDetails(resultView);
            Entities = result.Items;
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageIndex,
                TotalPages =  (int)Math.Ceiling((double)result.TotalCount / ItemsPerPage),
                PageSize = ItemsPerPage,
                Search = search
            };
            TagIds = guidTagIds;
            Radar.UI.SeoHelper.SetupListPostViewSeoDetails(ViewData, Entities);


            return Page();
        }
    }
}