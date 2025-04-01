using System.Text;

namespace InvestList.Middlewares;

public class GenerateCspHeader(RequestDelegate next)
{
    private static readonly string cspHeader; 

    static GenerateCspHeader()
    {
        var csp = new StringBuilder();

        // Default and script-src
        csp.Append("default-src 'self'; ");
        csp.Append("script-src 'self' 'unsafe-inline' ");

        csp.Append("https://cdn.jsdelivr.net https://code.jquery.com https://uicdn.toast.com ");
        // Preview Mode
        csp.Append("https://googletagmanager.com https://tagmanager.google.com ");
        
        // Google Analytic
        csp.Append("https://*.googletagmanager.com ");
        
        // Google Ads
        csp.Append("https://www.googleadservices.com https://www.google.com https://www.googletagmanager.com https://pagead2.googlesyndication.com https://googleads.g.doubleclick.net ");
        csp.Append(";");
        
        // Style src for Preview Mode
        csp.Append($"style-src 'self' 'unsafe-inline' ");
        csp.Append("https://cdn.jsdelivr.net https://uicdn.toast.com ");
        csp.Append("https://googletagmanager.com https://tagmanager.google.com https://fonts.googleapis.com ");
        csp.Append(";");
        
        // Font src for Preview Mode
        csp.Append("font-src 'self' ");
        csp.Append("https://fonts.gstatic.com data: ");
        csp.Append("https://cdn.jsdelivr.net");
        
        csp.Append(";");

        // Image sources
        csp.Append("img-src 'self' data: ");
        // Preview Mode
        csp.Append("https://googletagmanager.com https://tagmanager.google.com https://fonts.googleapis.com ");
        // Google Analytic
        csp.Append("https://*.google-analytics.com https://*.googletagmanager.com ");
        csp.Append("https://*.analytics.google.com https://*.googletagmanager.com https://*.g.doubleclick.net https://*.google.com https://*.google.ua ");
        // Google Ads
        csp.Append("https://www.googletagmanager.com https://googleads.g.doubleclick.net https://www.google.com https://google.com https://www.google.com.ua https://pagead2.googlesyndication.com ");
        csp.Append(";");
        
        // Connect sources
        csp.Append("connect-src 'self' ");
        // Google Analytic
        csp.Append("https://*.google-analytics.com https://*.analytics.google.com https://*.googletagmanager.com ");
        csp.Append("https://*.g.doubleclick.net https://*.google.com https://*.google.com.ua ");
        // Google Ads
        csp.Append("https://pagead2.googlesyndication.com https://www.googleadservices.com https://www.google.com https://google.com ");
        csp.Append(";");

        // Frame sources
        csp.Append("frame-src 'self' https://www.googletagmanager.com ");
        csp.Append("https://td.doubleclick.net https://www.googletagmanager.com ");
        csp.Append("https://googleads.g.doubleclick.net ");
        csp.Append(";");
        cspHeader = csp.ToString();
    }
    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Append("Content-Security-Policy", cspHeader);
        await next(context);
    }
}