using AutoMapper;
using Core.Interfaces;
using InvestList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestList.Areas.Main.Pages.Admin
{
    public class Index(ITagRepository tagRepository, IMapper mapper): PageModel
    {
        public IEnumerable<TagView> AvailableTags;
        
        public async Task OnGet()
        {
            var tags = await tagRepository.GetTags();
            AvailableTags = mapper.Map<IEnumerable<TagView>>(tags);
        }

        public async Task<IActionResult> OnPostAddTag(string tagName)
        {
            await tagRepository.Add(tagName);
            await OnGet();
            return Page();
        }
        
        public async Task<IActionResult> OnPostSubmitCustomHeader(List<Guid> tagIds)
        {
            await tagRepository.SubmitCustomHeader(tagIds);
            await OnGet();
            return Page();
        }
    }
}