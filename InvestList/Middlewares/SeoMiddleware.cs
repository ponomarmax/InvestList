using InvestList.Services;

namespace InvestList.Middlewares;

public class SeoMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var seoService = scope.ServiceProvider.GetRequiredService<ISeoService>();

            var pagePath = context.Request.Path.ToString();

            var seoDetails = await seoService.GetSeoDetailsAsync(pagePath);

            if (seoDetails != null)
            {
                context.Items["CustomTitle"] = seoDetails.PageTitle;
                context.Items["CustomPageH1"] = seoDetails.PageH1;
                context.Items["CustomDescription"] = seoDetails.MetaDescription;
            }
        }

        await next(context);
    }
}