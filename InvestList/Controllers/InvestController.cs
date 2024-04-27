using AutoMapper;
using Common;
using DataAccess.Interfaces;
using DataAccess.Models;
using InvestList.Filters;
using InvestList.Models;
using InvestList.Models.Invest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InvestList.Controllers
{
    [Authorize(Roles = $"{Const.BusinessRole},{Const.AdminRole}")]
    public class InvestController: Controller
    {
        private readonly INewsRepository _repository;
        private readonly IInvestAdRepository _investAdRepository;
        private readonly IMapper _mapper;
        private const int ItemsPerPage = 24; // Set the desired items per page

        public InvestController(IInvestAdRepository investAdRepository, IMapper mapper, INewsRepository repository)
        {
            _investAdRepository = investAdRepository;
            _mapper = mapper;
            _repository = repository;
        }

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
                : getUsdRange(filterModel.Currency, filterModel.MinInvestment, filterModel.MaxInvestment);
            var (count, resultDb) = await _investAdRepository.Filter(range.minUsd, range.maxUSd,
                filterModel?.MinAnnualInvestmentReturn, filterModel?.MaxAnnualInvestmentReturn, page, ItemsPerPage);
            if (!resultDb.Any())
            {
                return NotFound();
            }

            if (page != 1)
            {
                ViewData["DisplayNoIndexTag"] = true;
            }

            var resultView = _mapper.Map<IEnumerable<GetAllAdsView>>(resultDb);

            var totalPages = (int)Math.Ceiling((double)count / ItemsPerPage);

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(SearchRequestViewModel model)
        {
            var resultDb = await _investAdRepository.Search(model.SearchTerm, model.CurrentPage, ItemsPerPage);
            var resultView = _mapper.Map<IEnumerable<SearchResultViewModel>>(resultDb);


            var totalItems = (await _investAdRepository.Count())!;
            var totalPages = (int)Math.Ceiling((double)totalItems / ItemsPerPage);


            var viewModel = new ListSearchResultViewModel
            {
                Entities = resultView,
                SearchTerm = model.SearchTerm,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = model.CurrentPage,
                    TotalPages = totalPages,
                    PageSize = ItemsPerPage
                }
            };

            return View("Search", viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Filter(FilterRequestModel model)
        {
            var range = getUsdRange(model.Currency, model.MinInvestment, model.MaxInvestment);
            var (count, resultDb) = await _investAdRepository.Filter(range.minUsd, range.maxUSd,
                model.MinAnnualInvestmentReturn, model.MaxAnnualInvestmentReturn, model.CurrentPage, ItemsPerPage);
            var resultView = _mapper.Map<IEnumerable<GetAllAdsView>>(resultDb);

            var totalPages = (int)Math.Ceiling((double)count / ItemsPerPage);

            var viewModel = new ListInvestsViewModel
            {
                Entities = resultView,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = model.CurrentPage,
                    TotalPages = totalPages,
                    PageSize = ItemsPerPage
                }
            };

            return View("Index", viewModel);
        }

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

            var inv = _mapper.Map<InvestAd>(model);
            var invMeta = _mapper.Map<InvestAdExtraInfo>(model);
            await _investAdRepository.Create(inv, invMeta);


            return RedirectToAction("Details", new { id = inv.Id });
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var db = await _investAdRepository.Get(id);
            var result = _mapper.Map<InvestAdViewModel>(db);
            ViewData["CustomTitle"] = result.Title;
            return View(result);
        }

        // GET: InvestController/Edit/5
        [EmailConfirmedAuthorize]
        public async Task<ActionResult> Edit(Guid id)
        {
            var db = await _investAdRepository.Get(id);
            var result = _mapper.Map<PostInvestAdViewModel>(db);
            ViewData["Id"] = id;
            await PrepopulateCreate();
            return View(result);
        }

        [EmailConfirmedAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] PostInvestAdViewModel model)
        {
            var db = await _investAdRepository.Get(id);
            if (db == null)
                return null;
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                await PrepopulateCreate();
                return View("Edit", model);
            }

            var inv = _mapper.Map<InvestAd>(model);
            var invMeta = _mapper.Map<InvestAdExtraInfo>(model);
            inv.Id = id;
            await _investAdRepository.Edit(inv, invMeta);

            return RedirectToAction("Details", new { id = inv.Id });
        }

        private async Task PrepopulateCreate()
        {
            var userId = Utils.GetUserId(User);
            ViewData["UserId"] = userId;
            var dictionary = await _repository.GetTags();
            ViewData["Tags"] = dictionary;
        }
    }
}