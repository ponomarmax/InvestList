using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Radar.Domain.Entities;

namespace DataAccess.Repositories;

public interface ISeoRepository
{
    Task<SeoDetails> GetSeoDetailsAsync(string pagePath);
    Task UpdateSeoDetailsAsync(SeoDetails seoDetails);
    Task AddSeoDetailsAsync(SeoDetails seoDetails);
}

public class SeoRepository(ApplicationDbContext context) : ISeoRepository
{
    public async Task<SeoDetails> GetSeoDetailsAsync(string pagePath)
    {
        return await context.SeoDetails.FirstOrDefaultAsync(x => x.RelativePagePath == pagePath);
    }

    public async Task UpdateSeoDetailsAsync(SeoDetails seoDetails)
    {
        var existingSeoDetails = await context.SeoDetails.FindAsync(seoDetails.Id);
        if (existingSeoDetails != null)
        {
            existingSeoDetails.PageTitle = seoDetails.PageTitle;
            existingSeoDetails.MetaDescription = seoDetails.MetaDescription;
            existingSeoDetails.PageH1 = seoDetails.PageH1;
            context.SeoDetails.Update(existingSeoDetails);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddSeoDetailsAsync(SeoDetails seoDetails)
    {
        context.SeoDetails.Add(seoDetails);
        await context.SaveChangesAsync();
    }
}