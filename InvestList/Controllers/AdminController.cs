using AutoMapper;
using Core.Interfaces;
using InvestList.Filters;
using InvestList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestList.Controllers
{
    [Authorize(Roles = $"{Const.AdminRole}")]
    public class AdminController(ITagRepository tagRepository, IMapper mapper): Controller
    {
        public async Task<IActionResult> Index()
        {
            var tags = await tagRepository.GetTagsV2();
            return View("Index", mapper.Map<IEnumerable<TagView>>(tags));
        }
        
        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTag(string tagName)
        {
            await tagRepository.Add(tagName);
            return LocalRedirect("~/Admin/Index");
        }
        
        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitCustomHeader(List<Guid> tagIds)
        {
            await tagRepository.SubmitCustomHeader(tagIds);
            return LocalRedirect("~/Admin/Index");
        }
    }
}