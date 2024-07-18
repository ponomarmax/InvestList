using System.Text;
using Microsoft.AspNetCore.Mvc;
using InvestList.Services;

namespace InvestList.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ISitemapGenerator _sitemapGenerator;

        public SitemapController(ISitemapGenerator sitemapGenerator)
        {
            _sitemapGenerator = sitemapGenerator;
        }

        [Route("/sitemap.xml")]
        public IActionResult SitemapXml()
        {
            var t = $"{Request.Scheme}://{Request.Host.Value}/";
            var sitemapContent = _sitemapGenerator.GenerateSitemap(t);

            return Content(sitemapContent, "application/xml", Encoding.UTF8);
        }
    }
}
