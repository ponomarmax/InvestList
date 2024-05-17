using AutoMapper;
using DataAccess;
using InvestList.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestList.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagView>> GetCustomHeader();
    }

    public class TagService(IMapper mapper, ApplicationDbContext context): ITagService
    {
        public async Task<IEnumerable<TagView>> GetCustomHeader()
        {
            var header = await context.CustomHeaders.Include(x => x.Tag).ToListAsync();
            return mapper.Map<IEnumerable<TagView>>(header);
        }
    }
}