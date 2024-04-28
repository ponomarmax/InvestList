using Microsoft.AspNetCore.Mvc;

namespace InvestList.Controllers
{
    public class MetadataController:Controller
    {
        [HttpGet("robots.txt")]
        public ContentResult RobotsTxt()
        {
            var robotsContent = System.IO.File.ReadAllText("Metadata/robots.txt");
            return Content(robotsContent, "text/plain");
        }
    }
}