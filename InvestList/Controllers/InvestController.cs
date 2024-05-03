using AutoMapper;
using Common;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using InvestList.Filters;
using InvestList.Models;
using InvestList.Models.Invest;
using InvestList.Models.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InvestList.Controllers
{
    [Authorize(Roles = $"{Const.BusinessRole},{Const.AdminRole}")]
    public class InvestController(
        IInvestAdRepository investAdRepository,
        IMapper mapper,
        ITagRepository tagRepository,
        INewsRepository newsRepository)
        : Controller
    {
        private const int ItemsPerPage = 24; // Set the desired items per page
        private const int titleForIndex = 3;
        private const int titleForDescription = 5;
        private const int maxTitleSize = 65;
        private const int maxDescriptionSize = 160;

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1, FilterRequestModel filterModel = null)
        {
            ViewData["CustomTitle"] = "Бізнес шукає інвесторів";

            if (page < 1)
            {
                return NotFound();
            }

            var range = filterModel == null
                ? (null, null)
                : getUsdRange(filterModel.Currency?? Currency.UAH, filterModel.MinInvestment, filterModel.MaxInvestment);
            var tagIds = filterModel?.TagIds?.Where(x => Guid.TryParse(x, out _)).Select(Guid.Parse).ToList();
            var (count, resultDb) = await investAdRepository.Filter(range.minUsd, range.maxUSd,
                filterModel?.MinAnnualInvestmentReturn, filterModel?.MaxAnnualInvestmentReturn, page, ItemsPerPage,
                tagIds);
            if (!resultDb.Any())
            {
                return NotFound();
            }

            if (page != 1)
            {
                ViewData["DisplayNoIndexTag"] = true;
            }

            var resultView = mapper.Map<IEnumerable<GetAllAdsView>>(resultDb);


            var totalPages = (int)Math.Ceiling((double)count / ItemsPerPage);
            SetTitles(resultView);
            SetIndexPageDescription(resultView);
            var viewModel = new ListInvestsViewModel
            {
                Entities = resultView,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = ItemsPerPage
                },
                FilterModel = filterModel
            };

            return View("Index", viewModel);
        }


        // [HttpGet]
        // [AllowAnonymous]
        // public async Task<IActionResult> Search(SearchRequestViewModel model)
        // {
        //     var resultDb = await investAdRepository.Search(model.SearchTerm, model.CurrentPage, ItemsPerPage);
        //     var resultView = mapper.Map<IEnumerable<SearchResultViewModel>>(resultDb);
        //
        //
        //     var totalItems = (await investAdRepository.Count())!;
        //     var totalPages = (int)Math.Ceiling((double)totalItems / ItemsPerPage);
        //
        //
        //     var viewModel = new ListSearchResultViewModel
        //     {
        //         Entities = resultView,
        //         SearchTerm = model.SearchTerm,
        //         PaginationInfo = new PaginationInfo
        //         {
        //             CurrentPage = model.CurrentPage,
        //             TotalPages = totalPages,
        //             PageSize = ItemsPerPage
        //         }
        //     };
        //
        //     return View("Search", viewModel);
        // }

        // [HttpGet]
        // [AllowAnonymous]
        // public async Task<IActionResult> Filter(FilterRequestModel model)
        // {
        //     var range = getUsdRange(model.Currency, model.MinInvestment, model.MaxInvestment);
        //     var (count, resultDb) = await investAdRepository.Filter(range.minUsd, range.maxUSd,
        //         model.MinAnnualInvestmentReturn, model.MaxAnnualInvestmentReturn, model.CurrentPage, ItemsPerPage);
        //     var resultView = mapper.Map<IEnumerable<GetAllAdsView>>(resultDb);
        //
        //     var totalPages = (int)Math.Ceiling((double)count / ItemsPerPage);
        //
        //     var viewModel = new ListInvestsViewModel
        //     {
        //         Entities = resultView,
        //         PaginationInfo = new PaginationInfo
        //         {
        //             CurrentPage = model.CurrentPage,
        //             TotalPages = totalPages,
        //             PageSize = ItemsPerPage
        //         }
        //     };
        //
        //     return View("Index", viewModel);
        // }

        private (decimal? minUsd, decimal? maxUSd) getUsdRange(Currency currency, decimal? min, decimal? max)
        {
            if (currency == Currency.USD)
                return (min, max);
            return (min / 37, max / 37);
        }

        [EmailConfirmedAuthorize]
        public async Task<ActionResult> Create()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                await PrepopulateCreate();
                return View("Create");
            }

            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        [EmailConfirmedAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostInvestAdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PrepopulateCreate();
                return View("Create", model);
            }

            var inv = mapper.Map<InvestAd>(model);
            var invMeta = mapper.Map<InvestAdExtraInfo>(model);
            await investAdRepository.Create(inv, invMeta);


            return RedirectToAction("Details", new { id = inv.Id });
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var db = await investAdRepository.Get(id);
            if (db == null)
                return NotFound();
            var result = mapper.Map<InvestAdViewModel>(db);
            var tagIds = db.Tags?.Select(x => x.TagId).ToList();
            result.SimilarNews = mapper.Map<IEnumerable<GetNewsViewModel>>(await newsRepository.GetSimilarNews(tagIds));
            result.SimilarInvests =
                mapper.Map<IEnumerable<GetAllAdsView>>(await investAdRepository.GetSimilarInvest(tagIds));

            SetTitle(result);
            return View(result);
        }

        // GET: InvestController/Edit/5
        [EmailConfirmedAuthorize]
        public async Task<ActionResult> Edit(Guid id)
        {
            var db = await investAdRepository.Get(id);
            var result = mapper.Map<PostInvestAdViewModel>(db);
            ViewData["Id"] = id;
            await PrepopulateCreate();
            return View(result);
        }

        [EmailConfirmedAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] PostInvestAdViewModel model)
        {
            var db = await investAdRepository.Get(id);
            if (db == null)
                return null;
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                await PrepopulateCreate();
                return View("Edit", model);
            }

            var inv = mapper.Map<InvestAd>(model);
            var invMeta = mapper.Map<InvestAdExtraInfo>(model);
            inv.Id = id;
            await investAdRepository.Edit(inv, invMeta);

            return RedirectToAction("Details", new { id = inv.Id });
        }

        private async Task PrepopulateCreate()
        {
            var userId = Utils.GetUserId(User);
            ViewData["UserId"] = userId;
            var dictionary = await tagRepository.GetTags();
            ViewData["Tags"] = dictionary;
        }

        private void SetTitle(InvestAdViewModel entity)
        {
            ViewData["CustomTitle"] = maxTitleSize < entity.Title.Length
                ? entity.Title.Substring(0, maxTitleSize)
                : entity.Title;
            ViewData["CustomDescription"] = maxDescriptionSize < entity.Description?.Length?
                entity.Description?.Substring(0, maxDescriptionSize) :
                entity.Description;
        }

        private void SetTitles(IEnumerable<GetAllAdsView>? entities)
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

        private void SetIndexPageDescription(IEnumerable<GetAllAdsView>? entities)
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