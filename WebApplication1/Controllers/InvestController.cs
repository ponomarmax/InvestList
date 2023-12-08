using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InvestController: Controller
    {
        private readonly IInvestAdRepository _investAdRepository;
        private readonly IMapper _mapper;
        private readonly Dictionary<Guid, string> _investFields;
        private const int ItemsPerPage = 10; // Set the desired items per page

        public InvestController(IInvestAdRepository investAdRepository, IMapper mapper)
        {
            _investAdRepository = investAdRepository;
            _mapper = mapper;
            _investFields = _investAdRepository.GetFields().GetAwaiter().GetResult().ToDictionary(x => x.Id, y => y.Title);
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var resultDb = await _investAdRepository.GetAllShorted(page, ItemsPerPage);
            var resultView = _mapper.Map<IEnumerable<GetAllAdsView>>(resultDb);


            var totalItems = (await _investAdRepository.Count())!;
            var totalPages = (int)Math.Ceiling((double)totalItems / ItemsPerPage);


            var viewModel = new CurrentInvAdsListViewModel
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

        //[Authorize]
        public async Task<ActionResult> Create()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                PrepopulateCreate();
                return View("Create");

            }
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public async Task<IActionResult> Create(PostInvestAdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PrepopulateCreate();
                return View("Create", model);
            }

            var inv = _mapper.Map<InvestAd>(model);
            var invMeta = _mapper.Map<InvestAdExtraInfo>(model);
            await _investAdRepository.Create(inv, invMeta);


            //return View("Success");
            return RedirectToAction("Details", new { id = inv.Id });
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var db = await _investAdRepository.Get(id);
            var result = _mapper.Map<InvestAdViewModel>(db);
            return View(result);
        }

        // GET: InvestController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var db = await _investAdRepository.Get(id);
            var result = _mapper.Map<PostInvestAdViewModel>(db);
            ViewData["Id"] = id;
            PrepopulateCreate();
            return View(result);
        }

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
                PrepopulateCreate();
                return View("Edit", model);
            }

            var inv = _mapper.Map<InvestAd>(model);
            var invMeta = _mapper.Map<InvestAdExtraInfo>(model);
            inv.Id = id;
            await _investAdRepository.Edit(inv, invMeta);


            //return View("Success");
            return RedirectToAction("Details", new { id = inv.Id });
        }

        private void PrepopulateCreate()
        {
            var userId = GetCurrentUserId();
            ViewData["InvestFieldsOptions"] = _investFields;
            ViewData["UserId"] = userId;
        }

        private int CalculateTotalPages(int itemsPerPage, int totalItems)
        {
            return (int)Math.Ceiling((double)totalItems / itemsPerPage);
        }

        // GET: InvestController/Details/5


        public string GetCurrentUserId()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Retrieve the user ID from the claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    // Return the user ID
                    return userIdClaim.Value;
                }
            }
            throw new NullReferenceException("UserId null");
        }


        // GET: InvestController/Create
        //public ActionResult Create(Post post)
        //{
        //    _contextAccessor.Posts.Add(post);
        //    _contextAccessor.SaveChanges();
        //    return All();
        //}


        // GET: InvestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InvestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
