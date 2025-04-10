using System.Text;
using Microsoft.AspNetCore.Mvc;
using InvestList.Services;
using Radar.Domain.Interfaces;

namespace InvestList.Controllers
{
    public class SitemapController(ISitemapGenerator sitemapGenerator): Controller
    {
        [Route("/sitemap.xml")]
        public IActionResult SitemapXml()
        {
            var t = $"{Request.Scheme}://{Request.Host.Value}/";
            var sitemapContent = sitemapGenerator.GenerateSitemap(t);

            return Content(sitemapContent, "application/xml", Encoding.UTF8);
        }
    }
}
