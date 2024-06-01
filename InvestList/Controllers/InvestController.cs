using AutoMapper;
using Core.Interfaces;
using DataAccess.Interfaces;
using InvestList.Models.Invest;
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

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1, FilterRequestModel filterModel = null)
        {
        
            if (page < 1)
            {
                return NotFound();
            }
        
            return RedirectToPagePermanent("/Areas/Main/Pages/Invest/List", new { pageIndex = page, tagIds = filterModel?.TagIds });
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var db = await investAdRepository.Get(id);
            if (db == null)
                return NotFound();
            return RedirectToPagePermanent("/Invest/Get", new { area="Main", id = db.Slug });
        }
    }
}