using Core;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radar.Domain;
using Radar.Domain.Entities;
using Radar.Infrastructure.Repositories;

namespace DataAccess.Repositories
{
    public class InvestRepository(ApplicationDbContext dbContext) : BasePostRepository(dbContext), IInvestRepository
    {
        public async Task<(int count, IEnumerable<InvestPost> list)> Filter(PaginationData paginationData)
        {
            var basePosts = BasePosts(paginationData);
            var query = AsSplitQuery(paginationData, basePosts);

            var count = await basePosts.CountAsync();
            if (count <= 0)
                return (0, Array.Empty<InvestPost>());

            var items = await ComposeInvestPostQuery(query).ToListAsync();
            return (count, items);
        }

        public async Task<InvestPost?> Get(string slug, bool? loadDbImage = null)
        {
            var postQuery = QueryableGet(slug, PostType.InvestAd.ToString(), loadDbImage:loadDbImage);
            return await ComposeInvestPostQuery(postQuery).FirstOrDefaultAsync();
        }

        private IQueryable<InvestPost> ComposeInvestPostQuery(IQueryable<Post> postQuery)
        {
            return from inv in dbContext.InvestPosts
                join p in postQuery on inv.PostId equals p.Id
                join cur in dbContext.MinInvestValue on inv.Id equals cur.InvestPostId into curGroup
                orderby p.Priority descending, p.CreatedAt descending
                select new InvestPost
                {
                    Id = inv.Id,
                    PostId = inv.PostId,
                    Post = p,
                    TotalInvestment = inv.TotalInvestment,
                    InvestDurationMonths = inv.InvestDurationMonths,
                    InvestDurationYears = inv.InvestDurationYears,
                    AnnualInvestmentReturn = inv.AnnualInvestmentReturn,
                    MinInvestValues = curGroup.ToList()
                }
                ;
        }

        public async Task<bool> Exists(string slug)
        {
            return await dbContext.Posts.CountAsync(x => x.Slug == slug.ToLower()) > 0;
        }


        public async Task<InvestPost?> Get(Guid id)
        {
            return await Get(id.ToString());
        }

        public async Task<string> Put(Guid? id, InvestPost invest)
        {
            var investOrigin = id.HasValue ? (await Get(id.Value)) : null;
            invest.Post = Upsert(investOrigin?.Post, invest.Post);
            if (investOrigin == null)
            {
                dbContext.InvestPosts.Add(invest);
            }
            else
            {
                dbContext.Attach(investOrigin); // або:
                dbContext.Entry(investOrigin).State = EntityState.Modified;
                if (investOrigin.MinInvestValues != null)
                {
                    foreach (var value in investOrigin.MinInvestValues)
                    {
                        dbContext.Entry(value).State = EntityState.Modified;
                    }
                }

                invest.MapTo(investOrigin);
                dbContext.InvestPosts.Update(investOrigin);
            }

            await dbContext.SaveChangesAsync();
            return investOrigin == null ? invest.Post.Slug : investOrigin.Post.Slug;
        }

        public async Task<string> Create(InvestPost invest)
        {
            dbContext.InvestPosts.Add(invest);
            await dbContext.SaveChangesAsync();
            return invest.Post.Slug;
        }

       public async Task<Dictionary<string, List<(Post Post, InvestPost? Invest)>>> GetGroupedPostsWithInvestAsync(
    string language,
    string? search,
    List<Guid>? tagIds,
    CancellationToken ct)
{
    var languageParam = new SqlParameter("@language", language);
    var searchParam = string.IsNullOrWhiteSpace(search) ? null : new SqlParameter("@search", search);
    var tagParams = tagIds?.Select((tag, index) => new SqlParameter($"@tag{index}", tag)).ToArray() ?? [];

    var searchCondition = searchParam == null ? "" : "AND pt.Title LIKE '%' + @search + '%'";
    var tagCondition = tagParams.Length == 0
        ? ""
        : $"AND EXISTS (SELECT 1 FROM PostTags ptg2 WHERE ptg2.PostId = p.Id AND ptg2.TagId IN ({string.Join(", ", tagParams.Select(p => p.ParameterName))}))";

    var whereClause = $@"
        WHERE p.IsActive = 1
        {searchCondition}
        {tagCondition}
    ";

    var sql = $@"
        WITH FirstImagePerPost AS (
            SELECT *, ROW_NUMBER() OVER (PARTITION BY PostId ORDER BY Id) AS rn
            FROM ImageMetadata
        ),
        FirstMinInvestValuePerPost AS (
            SELECT *, ROW_NUMBER() OVER (PARTITION BY InvestPostId ORDER BY Id) AS rn
            FROM MinInvestValue
        ),
        TopRankedPosts AS (
            SELECT 
                p.Id AS PostId,
                p.Slug,
                p.PostType,
                p.Priority,
                p.CreatedAt,
                p.UpdatedAt,
                p.IsActive,

                i.Id AS InvestId,
                i.PostId AS InvestPostId,
                i.TotalInvestment,
                i.InvestDurationYears,
                i.InvestDurationMonths,
                i.AnnualInvestmentReturn,

                pt.Title AS TranslationTitle,
                pt.Description AS TranslationDescription,

                im.Id AS ImageId,
                ga.PageViews AS PageViews,
                mi.MinValue as MinValue,
                mi.Currency as Currency,

                ROW_NUMBER() OVER (
                    PARTITION BY p.PostType 
                    ORDER BY p.Priority DESC, p.CreatedAt DESC, p.Id DESC
                ) AS rn
            FROM Posts p
            LEFT JOIN InvestPosts i ON p.Id = i.PostId
            LEFT JOIN FirstMinInvestValuePerPost mi ON mi.InvestPostId = i.Id AND mi.rn = 1
            JOIN PostTranslation pt ON p.Id = pt.PostId AND pt.Language = @language
            LEFT JOIN FirstImagePerPost im ON p.Id = im.PostId AND im.rn = 1
            LEFT JOIN GoogleAnalyticPostViews ga ON ga.PostId = p.Id
            {whereClause}
        ),
        TopPostsWithTags AS (
            SELECT 
                trp.*,
                ptg.TagId,
                tt.Name AS TagTitle
            FROM TopRankedPosts trp
            LEFT JOIN PostTags ptg ON ptg.PostId = trp.PostId
            LEFT JOIN Tags t ON ptg.TagId = t.Id
            LEFT JOIN TagTranslation tt ON t.Id = tt.TagId AND tt.Language = @language
            WHERE trp.rn <= 4
        )
        SELECT * FROM TopPostsWithTags;
    ";

    var parameters = new List<SqlParameter> { languageParam };
    if (searchParam != null)
        parameters.Add(searchParam);
    parameters.AddRange(tagParams);

    var rawResults = await dbContext
        .Set<TopPostWithInvestResult>()
        .FromSqlRaw(sql, parameters.ToArray())
        .ToListAsync(ct);

    var tagGroups = rawResults
        .Where(r => r.TagId.HasValue)
        .GroupBy(r => r.PostId)
        .ToDictionary(
            g => g.Key,
            g => g
                .GroupBy(x => x.TagId)
                .Select(r =>
                {
                    var first = r.First();
                    return new PostTags
                    {
                        TagId = r.Key!.Value,
                        Tag = new Tag
                        {
                            Id = r.Key.Value,
                            Translations = first.TagTitle != null
                                ? new List<TagTranslation>
                                {
                                    new()
                                    {
                                        TagId = r.Key.Value,
                                        Name = first.TagTitle!,
                                        Language = language
                                    }
                                }
                                : []
                        }
                    };
                }).ToList()
        );

    var grouped = rawResults
        .GroupBy(r => r.PostType)
        .ToDictionary(
            g => g.Key,
            g => g
                .GroupBy(x => x.PostId)
                .Select(gr =>
                {
                    var first = gr.First();
                    var post = new Post
                    {
                        Id = first.PostId,
                        Slug = first.Slug,
                        PostType = first.PostType,
                        Priority = first.Priority,
                        UpdatedAt = first.UpdatedAt,
                        CreatedAt = first.CreatedAt,
                        IsActive = first.IsActive,
                        GoogleAnalyticPostView = first.PageViews.HasValue
                            ? new GoogleAnalyticPostView()
                            {
                                PageViews = first.PageViews.Value,
                            }
                            : null,
                        Translations = string.IsNullOrEmpty(first.TranslationTitle)
                            ? []
                            : new List<PostTranslation>
                            {
                                new()
                                {
                                    PostId = first.PostId,
                                    Title = first.TranslationTitle!,
                                    Description = first.TranslationDescription,
                                    Language = language
                                }
                            },
                        Images = first.ImageId.HasValue
                            ? [ new ImageMetadata { Id = first.ImageId.Value } ]
                            : [],
                        Tags = tagGroups.ContainsKey(first.PostId)
                            ? tagGroups[first.PostId]
                            : []
                    };

                    var invest = first.InvestId.HasValue
                        ? new InvestPost
                        {
                            Id = first.InvestId.Value,
                            PostId = first.InvestPostId!.Value,
                            TotalInvestment = first.TotalInvestment ?? 0,
                            InvestDurationYears = first.InvestDurationYears ?? 0,
                            InvestDurationMonths = first.InvestDurationMonths ?? 0,
                            AnnualInvestmentReturn = first.AnnualInvestmentReturn ?? 0,
                        }
                        : null;

                    if (!string.IsNullOrWhiteSpace(first.Currency))
                    {
                        invest.MinInvestValues =
                        [
                            new()
                                { Currency = (Currency)Enum.Parse(typeof(Currency), first.Currency, ignoreCase: true), MinValue = first.MinValue.Value }
                        ];
                    }

                    return (post, invest);
                }).ToList()
        );

    return grouped;
}
        // public async Task<Dictionary<string, List<(Post Post, InvestPost Invest)>>> GetGroupedPostsWithInvestAsync(
        //     CancellationToken ct)
        // {
        //     
        //     var query = dbContext.Posts
        //         .Where(p => p.IsActive)
        //         .Select(p => new
        //         {
        //             p,
        //             Invest = dbContext.InvestPosts.FirstOrDefault(i => i.PostId == p.Id),
        //             Rank = EF.Functions.RowNumber()
        //                 .Over(partitionBy: p.PostType, orderBy: p.Priority descending, p.UpdatedAt descending)
        //         })
        //         .Where(x => x.Rank <= 4);
        //     
        //     var groupedQuery =
        //         from p in Query()
        //         join i in dbContext.Set<InvestPost>() on p.Id equals i.PostId into investGroup
        //         from invest in investGroup.DefaultIfEmpty()
        //         group new { Post = p, Invest = invest } by p.PostType
        //         into g
        //         select new
        //         {
        //             PostType = g.Key,
        //             Posts = g.Take(4), // top 4 per group,
        //         };
        //     var listAsync = await groupedQuery.ToDictionaryAsync(x => x.PostType,
        //         x => x.Posts.Select(x => (x.Post, x.Invest)).ToList(), ct);
        //     return listAsync;
        // }
    }
}