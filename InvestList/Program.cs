using System.Reflection;
using System.Text;
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
using InvestList.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.LoadAppSettingAndEnvValues();
builder.ConfigureLogging();
Log.Logger = new LoggerConfiguration().ConfigureDefaultLogger(builder.Configuration).CreateLogger();

try
{
    
    // builder.Services.AddCors(options =>
    // {
    //     options.AddDefaultPolicy(
    //         builder =>
    //         {
    //             builder.WithOrigins("https://pagead2.googlesyndication.com").AllowAnyHeader().AllowAnyMethod();
    //         });
    // });
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
    // builder.Services.AddIdentity<User,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    //     .AddEntityFrameworkStores<ApplicationDbContext>();
    // builder.Services.AddControllersWithViews()
    //     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PostInvestAdViewModelValidator>())
    //     .AddRazorRuntimeCompilation();
    builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
    {
        // Set the default page for Razor Pages in an area
        options.Conventions.AddAreaPageRoute("Main", "/Index", "");
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
    builder.Services.AddTransient<ISitemapGenerator, SitemapGenerator>();
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

    // using (var scope = app.Services.CreateScope())
    // {
    //     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //     var sitemapGenerator = scope.ServiceProvider.GetRequiredService<ISitemapGenerator>();
    //     var sitemapXml = sitemapGenerator.GenerateSitemap();
    //     var filePath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "sitemap.xml");
    //     File.WriteAllText(filePath, sitemapXml);
    // }

    app.UseMiddleware<WwwRedirectMiddleware>();
    app.Use(async (context, next) =>
    {
        var csp = new StringBuilder();

        // Default and script-src
        csp.Append("default-src 'self'; ");
        csp.Append("script-src 'self' 'unsafe-inline' ");

        csp.Append("https://cdn.jsdelivr.net https://code.jquery.com ");
        // Preview Mode
        csp.Append("https://googletagmanager.com https://tagmanager.google.com ");
        
        // Google Analytic
        csp.Append("https://*.googletagmanager.com ");
        
        // Google Ads
        csp.Append("https://www.googleadservices.com https://www.google.com https://www.googletagmanager.com https://pagead2.googlesyndication.com https://googleads.g.doubleclick.net ");
        csp.Append(";");
        
        // Style src for Preview Mode
        csp.Append($"style-src 'self' 'unsafe-inline' ");
        csp.Append("https://cdn.jsdelivr.net ");
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

        // Finalize the Content Security Policy
        context.Response.Headers.Add("Content-Security-Policy", csp.ToString());
        await next();
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    var defaultRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
    if (defaultRoot == null)
        app.UseStaticFiles();
    else
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(defaultRoot, "wwwroot"))
        });

    app.UseRouting();
    // app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
        ;

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
