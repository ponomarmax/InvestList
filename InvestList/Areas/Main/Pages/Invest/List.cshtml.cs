using InvestList.Services.Invest.Queries;
using InvestList.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.UI.Models;

namespace InvestList.Areas.Main.Pages.Invest
{
    public class List(IMediator mediator): PageModel
    {
        private const int ItemsPerPage = 50;
        public IEnumerable<InvestPostShortDto> Entities { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public string Search { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, IEnumerable<string> tagIds = null,  string search = null)
        {
            if (pageIndex < 1)
            {
                return NotFound();
            }
            
            Search = search;

            var guidTagIds = tagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse);

            var command = new GetInvestPostListQuery()
            {
                Page = pageIndex,
                Search = search,
                TagIds = guidTagIds?.ToList() ?? [],
                PageSize = ItemsPerPage
            };
        
            var result = await mediator.Send(command);
            if (!result.Items.Any() && pageIndex != 1) return NotFound();
            
            Entities = result.Items;
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageIndex,
                TotalPages =  (int)Math.Ceiling((double)result.TotalCount / ItemsPerPage),
                PageSize = ItemsPerPage,
                Search = search
            };
            Radar.UI.SeoHelper.SetupListPostViewSeoDetails(ViewData, Entities.Select(x=>x.Post));


            return Page();
        }
    }
}