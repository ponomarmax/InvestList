using AutoMapper;
using Common;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using InvestList.Filters;
using InvestList.Models;
using InvestList.Models.Invest;
using InvestList.Models.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestList.Controllers
{
    [Authorize(Roles = $"{Const.AdminRole}")]
    public class NewsController(INewsRepository repository, IMapper mapper, ITagRepository tagRepository, IInvestAdRepository investAdRepository)
        : Controller
    {
        private const int ItemsPerPage = 24; // Set the desired items per page
        private const int titleForIndex = 3;
        private const int titleForDescription = 5;
        private const int maxTitleSize = 65;
        private const int maxDescriptionSize = 160;
        
        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1, FilterNewsRequestModel requestModel = null)
        {
            if (page < 1)
            {
                return NotFound();
            }
        
            return RedirectToPagePermanent("/Areas/Main/Pages/News/List", new { pageIndex = page, tagIds = requestModel?.TagIds });
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var db = await repository.Get(id);
            if (db == null)
                return NotFound();
            return RedirectToPagePermanent("/News/Get", new { area="Main", id = db.Slug });
        }

        // [EmailConfirmedAuthorize]
        // [HttpGet]
        // public async Task<ActionResult> Create()
        // {
        //     await PrepopulateCreate();
        //     return View("Create");
        // }
        //
        //
        // [HttpPost]
        // [EmailConfirmedAuthorize]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(PostNewsViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         await PrepopulateCreate();
        //         return View("Create", model);
        //     }
        //
        //     var news = mapper.Map<News>(model);
        //     await repository.Create(news);
        //
        //     return RedirectToAction("Details", new { id = news.Id });
        // }
        //
        // public async Task<ActionResult> Edit(Guid id)
        // {
        //     var db = await repository.Get(id);
        //     var result = mapper.Map<PostNewsViewModel>(db);
        //     ViewData["Id"] = id;
        //     await PrepopulateCreate();
        //     return View(result);
        // }
        //
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(Guid id, [FromForm] PostNewsViewModel model)
        // {
        //     var db = await repository.Get(id);
        //     if (db == null) return null;
        //     if (!ModelState.IsValid)
        //     {
        //         ViewData["Id"] = id;
        //         await PrepopulateCreate();
        //         return View("Edit", model);
        //     }
        //
        //     var inv = mapper.Map<News>(model);
        //     inv.Id = id;
        //     await repository.Edit(inv);
        //
        //     return RedirectToAction("Details", new { id = inv.Id });
        // }

        private async Task PrepopulateCreate()
        {
            var userId = Utils.GetUserId(User);
            ViewData["UserId"] = userId;
            var dictionary = await tagRepository.GetTags();
            ViewData["Tags"] = dictionary;
        }

        private void SetTitleAndDescription(GetNewsViewModel entity)
        {
            if (string.IsNullOrEmpty(entity.TitleSeo))
            {
                ViewData["CustomTitle"] = maxTitleSize < entity.Title.Length
                    ? entity.Title.Substring(0, maxTitleSize)
                    : entity.Title;
            }
            else
            {
                ViewData["CustomTitle"] = entity.TitleSeo;
            }
            
            if (string.IsNullOrEmpty(entity.DescriptionSeo))
            {
                ViewData["CustomDescription"] = maxDescriptionSize < entity.Description?.Length?
                    entity.Description?.Substring(0, maxDescriptionSize) :
                    entity.Description;
            }
            else
            {
                ViewData["CustomDescription"] = entity.DescriptionSeo;
            }
        }

        private void SetTitles(IEnumerable<GetNewsViewModel>? entities)
        {
            var finalTitle = "Інвестиційні оголошення";

            if (entities != null && entities.Any())
            {
                finalTitle = string.Join(' ', entities.Take(titleForIndex).Select(x => x.Title));
                if (maxTitleSize < finalTitle.Length)
                    finalTitle = finalTitle.Substring(0, maxTitleSize);
            }

            ViewData["CustomTitle"] = finalTitle;
        }

        private void SetIndexPageDescription(IEnumerable<GetNewsViewModel>? entities)
        {
            var finalTitle = "Бізнес шукає інвесторів в багатьох оголошеннях";
            if (entities != null && entities.Any())
            {
                finalTitle = string.Join(' ',
                    entities.Skip(titleForIndex).Take(titleForDescription).Select(x => x.Title));
                if (maxDescriptionSize < finalTitle.Length)
                    finalTitle = finalTitle.Substring(0, maxDescriptionSize);
            }

            ViewData["CustomDescription"] = finalTitle;
        }
    }
}