using System.Reflection;
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
using InvestList.Logging;
using InvestList.Services;
using InvestList.Validators;
using Microsoft.Extensions.FileProviders;
using DataAccess.Migrations;

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
        // context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        // context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        // context.Response.Headers.Add("Access-Control-Allow-Headers", "application/json");
        context.Response.Headers.Add("Content-Security-Policy",
            "default-src 'self'; script-src 'self' 'unsafe-inline' https://tpc.googlesyndication.com https://pagead2.googlesyndication.com https://code.jquery.com https://cdn.jsdelivr.net https://www.googletagmanager.com https://www.google-analytics.com; style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; font-src 'self' https://cdn.jsdelivr.net https://fonts.gstatic.com; img-src 'self' data: https://www.google-analytics.com https://pagead2.googlesyndication.com; frame-src 'self' https://www.google.com https://tpc.googlesyndication.com https://googleads.g.doubleclick.net https://pagead2.googlesyndication.com; object-src 'none'; connect-src 'self' https://www.google-analytics.com https://pagead2.googlesyndication.com;");
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
