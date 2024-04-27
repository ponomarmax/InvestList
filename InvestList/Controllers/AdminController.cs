using DataAccess.Repositories;
using InvestList.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestList.Controllers
{
    [Authorize(Roles = $"{Const.AdminRole}")]
    public class AdminController: Controller
    {
        private readonly ITagRepository _tagRepository;

        public AdminController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagRepository.GetTags();
            return View("Index", tags.Select(x=>x.Value).ToList());
        }
        
        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTag(string tagName)
        {
            await _tagRepository.Add(tagName);
            var tags = await _tagRepository.GetTags();
            return View("Index", tags.Select(x=>x.Value).ToList());
        }
    }
}