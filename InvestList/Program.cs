using System.Reflection;
using DataAccess;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using InvestList;
using InvestList.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Radar.Application.Posts.Commands;
using Radar.Domain.Entities;
using Radar.Infrastructure;
using Radar.Infrastructure.Configs;
using Radar.Infrastructure.Jobs;
using Radar.Infrastructure.Logging;
using Radar.Infrastructure.Middlewares;
using Radar.UI;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/startup-errors.txt")
    .CreateBootstrapLogger();

try
{
    Log.Information("🔧 Запуск застосунку...");

    var builder = WebApplication.CreateBuilder(args);
    builder.LoadAppSettingAndEnvValues();
    // -------------------- Logging --------------------
    builder.Host.UseSerilog((ctx, services, loggerConfig) =>
    {
        LogConfigurator.Configure(loggerConfig, ctx.Configuration);
    });
    
    var connectionString = builder.Configuration
        .GetSection("InvestRadar:ConnectionStrings")
        .GetValue<string>("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddHostedService<GoogleAnalyticJob>();
    
    builder.Services.Load<GoogleAnalyticsConfig>(builder.Configuration, "InvestRadar:GoogleAnalytics");
    
    builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>()
        .AddFluentValidationAutoValidation();
    builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
    {
        options.Conventions.Add(new GlobalCultureTemplatePageRouteModelConvention());
    });
    
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(CreatePostCommand).Assembly));
    
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

    builder.Services.ConfigureLocalisation();
    
    builder.Services.Configure<RazorViewEngineOptions>(options =>
    {
        options.PageViewLocationFormats.Add("/Pages/Shared/{0}.cshtml");
    });
    ;

    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddAutoMapper(typeof(Radar.Application.Mapping.DtoToEntityProfile).Assembly);
    
    builder.Services.Load<EmailConfig>(builder.Configuration, "InvestRadar:Email");
    builder.Services.Load<GoogleAnalyticsConfig>(builder.Configuration, "InvestRadar:GoogleAnalytics");
    builder.Services.AddInvestRadarServices();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
            var googleAuthNSection = builder.Configuration.GetSection("InvestRadar:Authentication:Google");
            options.ClientId = googleAuthNSection["ClientId"];
            options.ClientSecret = googleAuthNSection["ClientSecret"];
            
            // Save tokens for later use if needed
            options.SaveTokens = true;
        });
    builder.Services.ConfigureApplicationCookie(options => 
    { 
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    });
    Log.Logger.Information("App is starting");

    var app = builder.Build();

    // -------------------- Middleware --------------------
    app.UseMiddleware<ErrorHandlingMiddleware>();                 // Ловить всі помилки
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    
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
    
    app.UseStaticFiles();
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

    bool IsStaticFilePath(PathString path)
    {
        return path.StartsWithSegments("/images") ||
               path.StartsWithSegments("/css") ||
               path.StartsWithSegments("/js");
        // Add other static file prefixes as needed
    }
    
    // Middleware order is crucial
    app.UseRouting();

    // Add the ExternalAuthMiddleware before authentication
    // app.UseMiddleware<ExternalAuthMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    // Culture handling middleware after authentication
    app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
    app.UseMiddleware<LanguageRedirectMiddleware>();

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

    Log.Information("Middleware configuration completed, starting application");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application failed to start");
    throw;
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
}
