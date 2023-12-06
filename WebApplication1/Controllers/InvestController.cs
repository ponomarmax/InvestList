using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InvestController: Controller
    {
        private readonly IInvestAdRepository _investAdRepository;
        private readonly IMapper _mapper;

        public InvestController(IInvestAdRepository investAdRepository, IMapper mapper)
        {
            _investAdRepository = investAdRepository;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var resultDb = await _investAdRepository.GetAllShorted();
            var resultView = _mapper.Map<IEnumerable<GetAllAdsView>>(resultDb);
            return View(resultView);
        }

        //[Authorize]
        public async Task<ActionResult> Create()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = GetCurrentUserId();
                var dict = (await _investAdRepository.GetFields()).ToDictionary(x => x.Id, y => y.Title);
                ViewData["InvestFieldsOptions"] = new MultiSelectList(dict, "Key", "Value");
                ViewData["InvestFields"] = dict;
                var viewModel = new PostInvestAdViewModel
                {
                    AuthorId = userId,
                };

                return View("Create", viewModel);

            }
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public async Task<IActionResult> Create(PostInvestAdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Check ModelState errors and handle them if needed
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                // Log or debug the errors
                return View("Create", model); // Return the view with the model to display validation errors
            }

            if (ModelState.IsValid)
            {
                var inv = _mapper.Map<InvestAd>(model);
                var invMeta = _mapper.Map<InvestAdExtraInfo>(model);
                await _investAdRepository.Create(inv, invMeta);


                return RedirectToAction("Success");
            }

            // If the model is not valid, redisplay the form with validation errors
            return View("Create", model);
        }

        // GET: InvestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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


        // GET: InvestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InvestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
