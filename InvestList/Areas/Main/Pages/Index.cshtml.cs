using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models;
using InvestList.Models.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestList.Areas.Main.Pages
{
    public class Index(IPostRepository postRepository, IInvestRepository invRepository, IMapper mapper): PageModel
    {
        private const int ItemsPerPage = 4;

        public IEnumerable<PostView> BlacklistPosts { get; set; }
        public IEnumerable<PostView> NewsPosts { get; set; }
        public IEnumerable<InvestView> InvestPosts { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
        public string Search { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int pageIndex = 1,
            IEnumerable<string> tagIds = null,
            string search = null)

        {
            Search = search;
            if (pageIndex < 1)
            {
                return NotFound();
            }

            var guidTagIds = tagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse);

            var (_, postDb) = await invRepository.Filter(pageIndex, ItemsPerPage, guidTagIds, search);
            var (_, newsDb) = await postRepository.Filter(pageIndex, ItemsPerPage, guidTagIds, search, PostType.News);
            var (_, blacklist) = await postRepository.Filter(pageIndex, ItemsPerPage, guidTagIds, search, PostType.Blacklist);

            NewsPosts = mapper.Map<IEnumerable<PostView>>(newsDb);
            BlacklistPosts = mapper.Map<IEnumerable<PostView>>(blacklist);
            InvestPosts = mapper.Map<IEnumerable<InvestView>>(postDb);
            
            TagIds = guidTagIds;

            return Page();
        }
    }
}