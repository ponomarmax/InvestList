using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface ITagRepository
    {
        Task Add(string tagName);
        Task<Dictionary<Guid, string>> GetTags();
    }

    public class TagRepository: ITagRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TagRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new NullReferenceException("Empty tag");
            }

            _dbContext.Add(new Tag { Name = tagName });
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<Dictionary<Guid, string>> GetTags()
        {
            return (await _dbContext.Tags.ToListAsync()).ToDictionary(x => x.Id, x => x.Name);
        }
    }
}