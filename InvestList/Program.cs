using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Core.Entities;
using Core.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using InvestList;
using InvestList.Configs;
using InvestList.Extensions;
using InvestList.Jobs;
using InvestList.Logging;
using InvestList.Middlewares;
using InvestList.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Radar.Application;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;
using Radar.EF.Authorization;
using Radar.EF.Repositories;
using IImageService = Core.Interfaces.IImageService;
using IPostService = InvestList.Services.IPostService;

var builder = WebApplication.CreateBuilder(args);
builder.LoadAppSettingAndEnvValues();
builder.ConfigureLogging();
Log.Logger = new LoggerConfiguration().ConfigureDefaultLogger(builder.Configuration).CreateLogger();

try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddHostedService<GoogleAnalyticJob>();
    builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>()
        .AddFluentValidationAutoValidation();
    builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
    {
        // options.Conventions.AddAreaPageRoute("Main", "/Index", "");
        options.Conventions.Add(new GlobalCultureTemplatePageRouteModelConvention());
    });

    var supportedCultures = new[] { "uk", "en" };
    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        options.DefaultRequestCulture = new RequestCulture("uk");
        options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
        options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();

        // Use cookie provider to remember the user choice
        options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
    });
    
    
    builder.Services.Configure<RazorViewEngineOptions>(options =>
    {
        options.PageViewLocationFormats.Add("/Pages/Shared/{0}.cshtml");
    });
    ;

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.Load<EmailConfig>(builder.Configuration, "Email");
    builder.Services.AddTransient<IEmailSender, InvestList.Services.EmailSender>();
    builder.Services.AddScoped<IInvestRepository, InvestRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPostService, PostService>();
    builder.Services.AddScoped<IPostRepository, PostRepository>();
    builder.Services.AddScoped<ITagRepository, TagRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();
    builder.Services.AddSingleton<IInvestService, InvestService>();
    builder.Services.AddScoped<IImageService, ImageService>();
    builder.Services.AddScoped<ISeoRepository, SeoRepository>();
    builder.Services.AddScoped<ISeoService, SeoService>();
    builder.Services.AddTransient<ISitemapGenerator, SitemapGenerator>();
    builder.Services.AddScoped<IsAdminAuthorizationFilter>();
    builder.Services.AddScoped<IsPostOwnerAuthorizationFilter>();
    builder.Services.AddScoped<ITagService, TagService>();
    builder.Services.AddScoped<IBaseTagRepository, TagRepository>();
    builder.Services.AddScoped<IBaseUserRepository, UserRepository>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
            var googleAuthNSection =
                builder.Configuration.GetSection("Authentication:Google");
            options.ClientId = googleAuthNSection["ClientId"];
            options.ClientSecret = googleAuthNSection["ClientSecret"];
        });
    builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Identity/Account/Login"; });

    Log.Logger.Information("App is starting");

    var app = builder.Build();
    // using (var scope = app.Services.CreateScope())
    // {
    //     var i = scope.ServiceProvider.GetRequiredService<IImageService>();
    //     await i.LoadOnFileSystem();
    //     Log.Logger.Information("Images are load");
    // }

    app.UseMiddleware<WwwRedirectMiddleware>();
    app.UseMiddleware<GenerateCspHeader>();
    app.UseMiddleware<SeoMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    var rewriteOptions = new RewriteOptions()
        .AddRewrite(@"^(uk|en)/(images/.*)$", "$2", skipRemainingRules: true);
    app.UseRewriter(rewriteOptions);
    var defaultRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
    var wwwrootPath = defaultRoot == null ? builder.Environment.WebRootPath : Path.Combine(defaultRoot, "wwwroot");
    
    app.UseFileServer(new FileServerOptions
    {
        FileProvider = new PhysicalFileProvider(wwwrootPath),
        RequestPath = "",
        StaticFileOptions = { ServeUnknownFileTypes = true }
    });
    
    app.Use(async (context, next) =>
    {
        // Check if the response has started (i.e., file was served)
        if (!context.Response.HasStarted && IsStaticFilePath(context.Request.Path))
        {
            // If response hasn't started and it's a static file path, the file wasn't found
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("File not found");
            return; // End the request here
        }
        // Otherwise, proceed to the next middleware
        await next();
    });

// Helper method to identify static file paths
    bool IsStaticFilePath(PathString path)
    {
        return path.StartsWithSegments("/images") ||
               path.StartsWithSegments("/css") ||
               path.StartsWithSegments("/js");
        // Add other static file prefixes as needed
    }


    app.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value ?? "";
        var supportedCultures = new[] { "uk", "en" };
        var match = Regex.Match(path, @"^/(?<culture>uk|en)(/|$)");
        string preferredCulture = null;
        var cultureCookie = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
        if (!string.IsNullOrEmpty(cultureCookie))
        {
            var requestCulture = CookieRequestCultureProvider.ParseCookieValue(cultureCookie);
            preferredCulture = requestCulture?.Cultures.FirstOrDefault().Value;
        }
        if (string.IsNullOrEmpty(preferredCulture))
        {
            var userLanguages = context.Request.GetTypedHeaders().AcceptLanguage;
            preferredCulture = "uk";
            if (userLanguages != null)
            {
                foreach (var language in userLanguages)
                {
                    var lang = language.Value.Value;
                    if (lang.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                    {
                        preferredCulture = "en";
                        break;
                    }
                    else if (lang.StartsWith("uk", StringComparison.OrdinalIgnoreCase))
                    {
                        preferredCulture = "uk";
                        break;
                    }
                }
            }
        }
        if (!match.Success ||
            !match.Groups["culture"].Value.Equals(preferredCulture, StringComparison.OrdinalIgnoreCase))
        {
            if (match.Success)
            {
                path = path.Substring(match.Groups["culture"].Value.Length + 1);
                if (string.IsNullOrEmpty(path))
                {
                    path = "/";
                }
            }
            var queryString = context.Request.QueryString;
            context.Response.Redirect($"/{preferredCulture}{path}{queryString}", permanent: false);
            return;
        }
        await next();
    });
// 4. Then apply localization and routing for dynamic requests.
    app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

    app.UseRouting();
    // app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "language",
        pattern: "Language/SetLanguage", 
        defaults: new { controller = "Language", action = "SetLanguage" }
    );

    app.MapControllerRoute(
        name: "default",
        pattern: "{culture:regex(^[a-z]{{2}}$)}/{controller=Home}/{action=Index}/{id?}",
        defaults: new { culture = "uk" }
    );

    app.MapRazorPages();

    app.Run();
}
catch (Exception e)
{
    Log.Logger.Error(e, "App failed to run due to error");
}
finally
{
    Log.Logger.Information("App shutdowns");
}
