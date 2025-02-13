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
using InvestList.Jobs;
using InvestList.Logging;
using InvestList.Middlewares;
using InvestList.Services;
using Microsoft.Extensions.FileProviders;
using Radar.Domain.Entities;

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
    builder.Services.AddScoped<ISeoRepository, SeoRepository>();
    builder.Services.AddScoped<ISeoService, SeoService>();
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
