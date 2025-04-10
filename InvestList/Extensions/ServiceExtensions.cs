using Core.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using InvestList.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Radar.Application;
using Radar.Domain.Interfaces;
using Radar.Infrastructure;
using Radar.Infrastructure.Authorization;
using Radar.Infrastructure.Repositories;

namespace InvestList.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInvestRadarServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IInvestRepository, InvestRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddSingleton<IInvestService, InvestService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<ISeoRepository, SeoRepository>();
        services.AddScoped<ISeoService>(x=>new SeoService(x.GetRequiredService<ApplicationDbContext>()));
        services.AddTransient<ISitemapGenerator, SitemapGenerator>();
        services.AddScoped<IsAdminAuthorizationFilter>();
        services.AddScoped<IsPostOwnerAuthorizationFilter>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IBaseTagRepository, TagRepository>();
        services.AddScoped<IBaseUserRepository, UserRepository>();
        services.AddScoped<ISanitizerService, SanitizerService>();
        services.AddScoped<IPostFactoryService, PostFactoryService>();
        services.AddScoped<ISlugService, SlugService>();
        services.AddScoped<IBasePostRepository>(x=> new BasePostRepository(x.GetRequiredService<ApplicationDbContext>()));
        services.AddScoped<IGoogleAnalyticPostViewRepository>(x=> new GoogleAnalyticPostViewRepository(x.GetRequiredService<ApplicationDbContext>()));
        services.AddScoped<IImagePathBuilder, ImagePathBuilder>();
        services.AddScoped<ISlugResolver, SlugResolver>();
        return services;
    }
}