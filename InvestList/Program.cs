using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.V2;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using InvestList;
using InvestList.Configs;
using InvestList.Extensions;
using InvestList.Filters;
using InvestList.Logging;
using InvestList.Services;
using InvestList.Validators;

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

    builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    builder.Services
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PostInvestAdViewModelValidator>());
    // builder.Services.AddIdentity<User,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    //     .AddEntityFrameworkStores<ApplicationDbContext>();
    // builder.Services.AddControllersWithViews()
    //     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PostInvestAdViewModelValidator>())
    //     .AddRazorRuntimeCompilation();
    builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                // Set the default page for Razor Pages in an area
                options.Conventions.AddAreaPageRoute("Main", "/Invest/List", "");
            });;

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.Load<EmailConfig>(builder.Configuration, "Email");
    builder.Services.AddTransient<IEmailSender, InvestList.Services.EmailSender>();
    builder.Services.AddTransient<IInvestAdRepository, InvestAdRepository>();
    builder.Services.AddScoped<IInvestRepository, InvestRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<INewsRepository, NewsRepository>();
    builder.Services.AddScoped<ITagRepository, TagRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();
    builder.Services.AddScoped<ITagService, TagService>();
    builder.Services.AddScoped<IInvestService, InvestService>();
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
    app.UseMiddleware<WwwRedirectMiddleware>();
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Content-Security-Policy",
            "default-src 'self'; script-src 'self' 'unsafe-inline' https://code.jquery.com https://cdn.jsdelivr.net https://www.googletagmanager.com https://www.google-analytics.com; " +
            "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; font-src 'self' https://cdn.jsdelivr.net https://fonts.gstatic.com; img-src 'self' data: https://www.google-analytics.com; frame-src 'self'; object-src 'none'; connect-src 'self' https://www.google-analytics.com;");
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
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
    });

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