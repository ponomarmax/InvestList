using System.Globalization;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Radar.Domain.Entities;
using Radar.Infrastructure.Repositories;

namespace DataAccess.Repositories
{
    public class TagRepository(ApplicationDbContext dbContext): BaseTagRepository(dbContext), ITagRepository
    {

        private static Tag[] _tags = [];
        private static Dictionary<string, CustomHeader[]>? CustomHeader = new();
        
        public async Task Add(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new NullReferenceException("Empty tag");
            }

            // dbContext.Add(new Tag { Name = tagName });
            await dbContext.SaveChangesAsync();
            _tags =  await dbContext.Tags.ToArrayAsync();
        }

        public async Task SubmitCustomHeader(List<Guid> tagIds)
        {
            await dbContext.Database.BeginTransactionAsync();
            try
            {
                var t = dbContext.CustomHeaders.ToList();
                dbContext.CustomHeaders.RemoveRange(t);
                dbContext.CustomHeaders.AddRange(tagIds.Select(x => new CustomHeader { TagId = x }));
                await dbContext.SaveChangesAsync();
                await dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }

            // _customeHeader = await dbContext.CustomHeaders.Include(x => x.Tag).ToArrayAsync();
        }
        
        public async Task<IEnumerable<CustomHeader>> GetCustomHeader(string language)
        {
            if (string.IsNullOrWhiteSpace(language) || CustomHeader == null)
                throw new NullReferenceException("Empty custom header");
            
            if (CustomHeader.TryGetValue(language, out CustomHeader[] headers) && headers!=null && headers.Length > 0)
            {
                return headers;
            }
            CustomHeader[language] = await dbContext.CustomHeaders.Include(x => x.Tag).ThenInclude(x=>x.Translations.Where(x=>x.Language==language)).ToArrayAsync();
            return CustomHeader[language];
        }
    }
}