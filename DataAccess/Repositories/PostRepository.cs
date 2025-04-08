using Radar.Infrastructure.Repositories;

namespace DataAccess.Repositories;
public class PostRepository(ApplicationDbContext dbContext):BasePostRepository(dbContext)
{
    
}