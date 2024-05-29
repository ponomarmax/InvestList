using System;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class move_invest_to_post : Migration
    {
        /// <inheritdoc />
        private void ConfigureOptions(DbContextOptionsBuilder optionsBuilder)
        {
            // Load the connection string from appsettings.json
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        true, true)
                    .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables().Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public void CustomMigration()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ConfigureOptions(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var slugs = new HashSet<string>();

                foreach (var investAd in context.InvestAds.Include(x=>x.Tags).Include(x=>x.Comments)
                             .AsNoTracking())
                {
                    var history = context.InvestAdExtraInfo.Include(x=>x.AcceptedCurrencies).Where(x => x.InvestAdId == investAd.Id)
                        .OrderBy(x => x.CreatedAt).Last();
                    var slugCandidate = SlugGenerator.Get(history.Title);

                    
                    var post = new Post
                    {
                        Id = investAd.Id,
                        CreatedAt = investAd.CreatedAt.DateTime,
                        CreatedById = investAd.AuthorId,
                        Slug = investAd.Slug,
                        Title = history.Title,
                        Description = history.Description,
                        PostType = PostType.InvestAd,
                        IsActive = investAd.Published,
                        Comments =  investAd.Comments.Select(x=>new PostComment()
                        {
                            PostId = investAd.Id,
                            CreatedAt = x.CreatedAt.DateTime,
                            Text = x.Text,
                            UserId=x.UserId
                        }).ToList(),
                        Tags = investAd.Tags.Select(x=>new PostTags(){PostId = investAd.Id, TagId = x.TagId}).ToList()
                        
                        
                    };

                    
                    if (!string.IsNullOrEmpty(history.ImageBase64))
                    {
                        post.Images = new[] { new Image() { ImageBase64 = history.ImageBase64 } };
                    }

                    var investPost = new InvestPost()
                    {
                        MinInvestValues = history.AcceptedCurrencies.Select(x=> new MinInvestValue(){Currency = x.Currency, MinValue = x.MinValue}).ToList(),
                        AnnualInvestmentReturn = history.AnnualInvestmentReturn,
                        InvestDurationMonths = history.InvestDurationMonths,
                        InvestDurationYears = history.InvestDurationYears,
                        TotalInvestment = history.TotalInvestment,
                        Post = post
                    };

                    context.InvestPosts.Add(investPost);

                }

                context.SaveChanges();
            }
        }
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CustomMigration();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
