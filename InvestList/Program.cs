using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using InvestList;
using InvestList.Configs;
using InvestList.Extensions;
using InvestList.Logging;
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

    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.AddControllersWithViews()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PostInvestAdViewModelValidator>())
        .AddRazorRuntimeCompilation();
    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.Load<EmailConfig>(builder.Configuration, "Email");
    builder.Services.AddTransient<IEmailSender, InvestList.Services.EmailSender>();
    builder.Services.AddTransient<IInvestAdRepository, InvestAdRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<INewsRepository, NewsRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();
    Log.Logger.Information("App is starting");

    var app = builder.Build();
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

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Invest}/{action=Index}/{id?}");
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