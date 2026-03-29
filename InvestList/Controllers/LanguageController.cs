
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
namespace InvestList.Controllers;

public class LanguageController : Controller
{
    [HttpGet]
    public IActionResult SetLanguage([FromQuery] string culture, string returnUrl)
    {
        // Set the cookie for persistence.
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        // If returnUrl is null or empty, fallback to the current request's path and query string.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
        }

        // Normalize returnUrl: ensure it starts with "/"
        if (!returnUrl.StartsWith("/"))
        {
            returnUrl = "/" + returnUrl;
        }

        // Replace the current two-letter culture prefix with the new culture.
        // This assumes that the URL is always in the format "/{currentCulture}/{rest-of-path}"
        if (returnUrl.Length >= 3)
        {
            returnUrl = "/" + culture + returnUrl.Substring(3);
        }
        else
        {
            returnUrl = "/" + culture;
        }

        return LocalRedirect(returnUrl);
    }
}