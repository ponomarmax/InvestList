using System.Globalization;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.Application.Models;

namespace InvestList.Areas.Main.Pages.News
{
    public class List(IPostRepository repository, IMapper mapper): PageModel
    {
        private const int ItemsPerPage = 50;

        public IEnumerable<PostView> Entities { get; set; }
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

            var guidTagIds = tagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse);

            var (count, resultDb) = await repository.Filter(pageIndex, ItemsPerPage, CultureInfo.CurrentCulture.ToString(), guidTagIds, search, PostType.News);
            if (!resultDb.Any() && pageIndex != 1) return NotFound();

            var resultView = mapper.Map<IEnumerable<PostView>>(resultDb);

            var totalPages = (int)Math.Ceiling((double)count / ItemsPerPage);
            // ViewData.SetupListPostViewSeoDetails(resultView);
            Entities = resultView;
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageIndex,
                TotalPages = totalPages,
                PageSize = ItemsPerPage,
                Search = search
            };
            TagIds = guidTagIds;

            return Page();
        }
    }
}