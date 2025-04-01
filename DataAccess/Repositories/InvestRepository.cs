using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Radar.Domain;
using Radar.EF.Repositories;

namespace DataAccess.Repositories
{
    public class InvestRepository(ApplicationDbContext dbContext, IMapper mapper, IImageService imageService):BasePostRepository<Post>(dbContext), IInvestRepository
    {
        public async Task<(int count, IEnumerable<InvestPost> list)> Filter(int page,
            int offset,
            string language,
            IEnumerable<Guid>? tagIds,
            string search = null)
        {
            
            var request = new PaginationData()
            {
                Page = page,
                Language = language,
                PostType = Enum.GetName(typeof(PostType), PostType.InvestAd),
                Search = search,
                TagsIds = tagIds?.ToList(),
                Take = offset
            };

            var basePosts = BasePosts(request);
            var query = AsSplitQuery(request, basePosts); 
            
            var count = await basePosts.CountAsync();

            var t = from inv in dbContext.InvestPosts 
                join  p in query
                    on inv.PostId equals p.Id 
                select new {inv, p};

            if (count <= 0) return (0, Array.Empty<InvestPost>());
            var ttt = await t.ToListAsync();
            var re = new List<InvestPost>();
            ttt.ForEach(x =>
            {
                x.inv.Post = x.p;
                re.Add(x.inv);
            });
            return (count, re);
        }

        public async Task<InvestPost?> Get(string slug)
        {
            var post = QueryableGet(slug, PostType.InvestAd.ToString());
            var t = from inv in dbContext.InvestPosts
                join p in post on inv.PostId equals p.Id
                join cur in dbContext.MinInvestValue on inv.Id equals cur.InvestPostId into curGroup
                select new { inv, p, curList = curGroup.ToList() };
           
            var invAndPost =await t.FirstOrDefaultAsync();
            if (invAndPost == null) return null;
            
            invAndPost.inv.Post = invAndPost.p;
            invAndPost.inv.MinInvestValues = invAndPost.curList;
            return invAndPost.inv;
        }

        public async Task<bool> Exists(string slug)
        {
            return await dbContext.Posts.CountAsync(x => x.Slug == slug.ToLower()) > 0;
        }


        public async Task<InvestPost?> Get(Guid id)
        {
            return await Get(id.ToString());
        }

        public async Task<Guid> Put(Guid? id, InvestPost invest)
        {
            var investOrigin = id.HasValue ? (await Get(id.Value)) : null;
            invest.Post = Upsert(investOrigin?.Post, invest.Post);
            if (investOrigin == null)
            {
                dbContext.InvestPosts.Add(invest);
            }
            else
            {
                invest.MapTo(investOrigin);
                dbContext.InvestPosts.Update(investOrigin);
            }
            await dbContext.SaveChangesAsync();
            return investOrigin == null? invest.Post.Id : investOrigin.Post.Id;
        }

        public async Task Create(InvestPost invest)
        {
            dbContext.InvestPosts.Add(invest);
            await dbContext.SaveChangesAsync();
            var postOrigin = await Get(invest.Post.Slug);
            imageService.RefreshImages(postOrigin.Post, null);
        }
    }
}