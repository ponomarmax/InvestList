using Core.Entities;
using DataAccess;
using DataAccess.Repositories;
using InvestList.Areas.Main.Pages.Admin;
using Microsoft.EntityFrameworkCore;
using Radar.Domain.Entities;

namespace InvestList.Services;

public interface ISeoService
{
    Task<SeoDetails> GetSeoDetailsAsync(string pagePath);
    Task<List<SeoDetails>> GetAllSeoDetailsAsync();
    Task SaveSeoDetailsAsync(SeoDetails seoDetails);
    Task DeleteSeoDetailsAsync(Guid id);
}

public class SeoService(ApplicationDbContext context) : ISeoService
{
    public async Task<SeoDetails> GetSeoDetailsAsync(string pagePath)
    {
        var seoDetails = await context.SeoDetails.FirstOrDefaultAsync(s => s.RelativePagePath==pagePath);
        return seoDetails;
    }

    public async Task<List<SeoDetails>> GetAllSeoDetailsAsync()
    {
        return await context.SeoDetails.ToListAsync();
    }

    public async Task SaveSeoDetailsAsync(SeoDetails seoDetails)
    {
        var existing = context.SeoDetails.FirstOrDefault(s => s.RelativePagePath==seoDetails.RelativePagePath);
        if (existing != null)
        {
            // If SEO details exist, update them
            existing.PageTitle = seoDetails.PageTitle;
            existing.MetaDescription = seoDetails.MetaDescription;
            existing.PageH1 = seoDetails.PageH1;
        }
        else
        {
            seoDetails.Id = Guid.NewGuid();
            seoDetails.CreatedAt = DateTime.UtcNow;
            context.SeoDetails.Add(seoDetails);
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteSeoDetailsAsync(Guid id)
    {
        var seoDetails = context.SeoDetails.FirstOrDefault(s => s.Id == id);
        if (seoDetails != null)
        {
            context.SeoDetails.Remove(seoDetails);
        }
        await context.SaveChangesAsync();
    }
}