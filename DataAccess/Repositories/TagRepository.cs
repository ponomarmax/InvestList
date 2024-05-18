using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public interface ITagRepository
    {
        Task Add(string tagName);
        Task<Dictionary<Guid, string>> GetTags();
        Task SubmitCustomHeader(List<Guid> tagIds);
        Task<IEnumerable<Tag>> GetTagsV2();
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
        
        public async Task<IEnumerable<Tag>> GetTagsV2()
        {
            return await _dbContext.Tags.ToListAsync();
        }

        public async Task SubmitCustomHeader(List<Guid> tagIds)
        {
            await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var t = _dbContext.CustomHeaders.ToList();
                _dbContext.CustomHeaders.RemoveRange(t);
                _dbContext.CustomHeaders.AddRange(tagIds.Select(x => new CustomHeader { TagId = x }));
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }
}