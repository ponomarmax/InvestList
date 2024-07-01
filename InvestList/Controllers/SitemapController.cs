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
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sitemap.xml");
            System.IO.File.WriteAllText(filePath, sitemapContent);

            return Content(sitemapContent, "application/xml");
        }
    }
}
