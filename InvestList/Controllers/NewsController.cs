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
        private const int minDescriptionCharCount = 300;

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1, FilterNewsRequestModel requestModel = null)
        {
            if (page < 1)
            {
                return NotFound();
            }

            var tagIds = requestModel?.TagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse).ToList();
            var resultDb = await repository.GetPage(page, ItemsPerPage,
                tagIds);
            if (!resultDb.Any())
            {
                return NotFound();
            }

            var resultView = mapper.Map<IEnumerable<GetNewsViewModel>>(resultDb);
            if (page != 1)
            {
                ViewData["DisplayNoIndexTag"] = true;
            }

            var totalItems = (await repository.Count(tagIds))!;
            var totalPages = (int)Math.Ceiling((double)totalItems / ItemsPerPage);

            SetTitles(resultView);
            SetDescription(resultView);

            var viewModel = new ListNewsViewModel
            {
                Entities = resultView,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = ItemsPerPage
                },
                FilterModel = requestModel
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var db = await repository.Get(id);
            var tagIds = db.Tags.Select(x=>x.TagId).ToList();
            var similarNews = await repository.GetSimilarNews(tagIds);
            var result = mapper.Map<GetNewsViewModel>(db);
            var similarNewsViewModels = mapper.Map<IEnumerable<GetNewsViewModel>>(similarNews);
            var similarAds = mapper.Map<IEnumerable<GetAllAdsView>>(await investAdRepository.GetSimilarInvest(tagIds));
            result.SimilarNews = similarNewsViewModels;
            result.SimilarInvests = similarAds;
            SetTitle(result);
            return View(result);
        }

        [EmailConfirmedAuthorize]
        public async Task<ActionResult> Create()
        {
            await PrepopulateCreate();
            return View("Create");
        }


        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostNewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PrepopulateCreate();
                return View("Create", model);
            }

            var news = mapper.Map<News>(model);
            await repository.Create(news);

            return RedirectToAction("Details", new { id = news.Id });
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            var db = await repository.Get(id);
            var result = mapper.Map<PostNewsViewModel>(db);
            ViewData["Id"] = id;
            await PrepopulateCreate();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] PostNewsViewModel model)
        {
            var db = await repository.Get(id);
            if (db == null) return null;
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                await PrepopulateCreate();
                return View("Edit", model);
            }

            var inv = mapper.Map<News>(model);
            inv.Id = id;
            await repository.Edit(inv);

            return RedirectToAction("Details", new { id = inv.Id });
        }

        private async Task PrepopulateCreate()
        {
            var userId = Utils.GetUserId(User);
            ViewData["UserId"] = userId;
            var dictionary = await tagRepository.GetTags();
            ViewData["Tags"] = dictionary;
        }

        private void SetTitle(GetNewsViewModel entity)
        {
            ViewData["CustomTitle"] = entity.Title;
            ViewData["CustomDescription"] =
                entity.Description?.Substring(0, Math.Min(minDescriptionCharCount, entity.Description.Length)) ??
                entity.Title;
        }

        private void SetTitles(IEnumerable<GetNewsViewModel>? entities)
        {
            var finalTitle = "Останні новини зі світу інвестицій";
            if (entities != null && entities.Any())
            {
                finalTitle = $"{finalTitle}: {string.Join(' ', entities.Take(titleForIndex).Select(x => x.Title))}";
            }

            ViewData["CustomTitle"] = finalTitle;
        }

        private void SetDescription(IEnumerable<GetNewsViewModel>? entities)
        {
            var finalTitle = "Інвестиційні сенсації";
            if (entities != null && entities.Any())
            {
                finalTitle =
                    $"{finalTitle}: {string.Join(' ', entities.Skip(titleForIndex).Take(titleForDescription).Select(x => x.Title))}";
            }

            ViewData["CustomDescription"] = finalTitle;
        }
    }
}