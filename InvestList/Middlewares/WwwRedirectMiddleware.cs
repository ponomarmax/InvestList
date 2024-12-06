namespace InvestList.Middlewares
{
    public class WwwRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public WwwRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Host.Value.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                var newHost = context.Request.Host.Value.Replace("www.", string.Empty);
                var newUrl = $"{context.Request.Scheme}://{newHost}{context.Request.Path}{context.Request.QueryString}";
                context.Response.Redirect(newUrl, permanent: true);
                return;
            }

            await _next(context);
        }
    }
}