using AutoMapper;
using Common;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Filters;
using WebApplication1.Models;
using WebApplication1.Models.News;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = $"{Const.AdminRole}")]
    public class NewsController: Controller
    {
        private readonly INewsRepository _repository;
        private readonly IMapper _mapper;
        private const int ItemsPerPage = 24; // Set the desired items per page

        public NewsController(INewsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1)
        {
            var resultDb = await _repository.GetPage(page, ItemsPerPage);
            var resultView = _mapper.Map<IEnumerable<GetNewsViewModel>>(resultDb);


            var totalItems = (await _repository.Count())!;
            var totalPages = (int)Math.Ceiling((double)totalItems / ItemsPerPage);


            var viewModel = new ListNewsViewModel
            {
                Entities = resultView,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = ItemsPerPage
                }
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var db = await _repository.Get(id);
            var similarNews = await _repository.GetSimilarNews(id);
            var result = _mapper.Map<GetNewsViewModel>(db);
            var similarNewsViewModels = _mapper.Map<IEnumerable<GetNewsViewModel>>(similarNews);
            result.SimilarNews = similarNewsViewModels;
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

            var news = _mapper.Map<News>(model);
            await _repository.Create(news);
            
            return RedirectToAction("Details", new { id = news.Id });
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
