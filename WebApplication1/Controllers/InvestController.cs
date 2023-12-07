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

        public InvestController(IInvestAdRepository investAdRepository, IMapper mapper)
        {
            _investAdRepository = investAdRepository;
            _mapper = mapper;
            _investFields = _investAdRepository.GetFields().GetAwaiter().GetResult().ToDictionary(x => x.Id, y => y.Title);
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


            return View("Success");
        }

        private void PrepopulateCreate()
        {
            var userId = GetCurrentUserId();
            ViewData["InvestFieldsOptions"] = _investFields;
            ViewData["UserId"] = userId;
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
