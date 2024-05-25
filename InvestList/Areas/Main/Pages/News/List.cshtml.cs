using AutoMapper;
using DataAccess.Repositories.V2;
using InvestList.Models;
using InvestList.Models.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestList.Areas.Main.Pages.News
{
    public class List(IPostRepository repository, IMapper mapper): PageModel
    {
        private const int ItemsPerPage = 50;

        public IEnumerable<PostView> Entities { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
        public PaginationInfo PaginationInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, IEnumerable<string> tagIds = null)
        {
            if (pageIndex < 1)
            {
                return NotFound();
            }

            var guidTagIds = tagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse);

            var (count, resultDb) = await repository.Filter(pageIndex, ItemsPerPage, guidTagIds);
            if (!resultDb.Any() && pageIndex != 1) return NotFound();

            var resultView = mapper.Map<IEnumerable<PostView>>(resultDb);

            var totalPages = (int)Math.Ceiling((double)count / ItemsPerPage);
            ViewData.SetupListPostViewSeoDetails(resultView);
            Entities = resultView;
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageIndex,
                TotalPages = totalPages,
                PageSize = ItemsPerPage
            };
            TagIds = guidTagIds;

            return Page();
        }
    }
}