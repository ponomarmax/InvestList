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
            var sitemapContent = _sitemapGenerator.GenerateSitemap();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sitemap.xml");
            System.IO.File.WriteAllText(filePath, sitemapContent);

            return Content(sitemapContent, "application/xml");
        }
    }
}
