using AutoMapper;
using Core.Interfaces;
using InvestList.Models;
using InvestList.Models.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvestList.Areas.Main.Pages.Admin
{
    [Authorize(Roles = Const.AdminRole)]
    public class Index(ITagRepository tagRepository, IMapper mapper, IPostRepository postRepository): PageModel
    {
        public IEnumerable<TagView> AvailableTags;
        public IEnumerable<AdminPostView> Posts { get; set; }
        public PutAdminPost Post { get; set; }
        
        public async Task OnGet()
        {
            var tags = await tagRepository.GetTags();
            // var (count, posts) = await postRepository.Filter(1, int.MaxValue, null);
            // Posts = mapper.Map<IEnumerable<AdminPostView>>(posts); 
            // AvailableTags = mapper.Map<IEnumerable<TagView>>(tags);
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
        
        public async Task<IActionResult> OnPostUpdatePriority(PutAdminPost Post)
        {
            await postRepository.SetPriority(Post.Id, Post.Priority);
            // await tagRepository.SubmitCustomHeader(tagIds);
            await OnGet();
            return Page();
        }
    }
}