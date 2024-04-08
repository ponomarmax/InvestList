using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.Configs;
using WebApplication1.Extensions;
using WebApplication1.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true);

// Add services to the container.
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
builder.Services.AddTransient<IEmailSender, WebApplication1.Services.EmailSender>();
builder.Services.AddTransient<IInvestAdRepository, InvestAdRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
Log.Logger.Information("App is starting");
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});
var app = builder.Build();

Log.CloseAndFlush();

// Configure the HTTP request pipeline.
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
