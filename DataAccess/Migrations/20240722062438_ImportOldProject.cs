using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ImportOldProject
    {
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

        public List<InvestPost> ReadCsvManually(string filePath)
        {
            var posts = new List<InvestPost>();

            using (var reader = new StreamReader(filePath))
            {
                // Skip the header line if present
                string headerLine = reader.ReadLine();
        
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t');

                    var post = new Post
                    {
                        // Assuming CSV columns are in the order: Id, Title, Content, CreatedAt
                        Title = values[0],
                        Description = values[6].Trim('"'),
                        
                        CreatedAt = DateTime.Parse(values[8]),
                        UpdatedAt = DateTime.Parse(values[8]),
                        CreatedById = "168e0e85-e7f5-41df-bdd0-404672a477ae",
                        PostType = PostType.InvestAd,
                        IsActive = true,
                        Slug = SlugGenerator.Get(values[0])
                        // Content = values[2],
                        // CreatedAt = DateTime.Parse(values[3], CultureInfo.InvariantCulture)
                    };
                    if (!string.IsNullOrWhiteSpace(values[7]))
                    {
                        post.Tags = new[] { new PostTags() { TagId = Guid.Parse(values[7]) } };
                    }
                    var invest = new InvestPost()
                    {
                        AnnualInvestmentReturn = decimal.Parse(values[1]),
                        InvestDurationYears = int.TryParse(values[4], out var years)?years:0,
                        InvestDurationMonths = int.TryParse(values[5], out var month)?month:0,
                    };
                    if (string.IsNullOrWhiteSpace(values[2]))
                    {
                        invest.MinInvestValues = new[]
                            { new MinInvestValue() { Currency = Currency.UAH, MinValue = decimal.Parse(values[3]) } };
                    }
                    else
                    {
                        invest.MinInvestValues = new[]
                            { new MinInvestValue() { Currency = Currency.USD, MinValue = decimal.Parse(values[2]) } };
                    }

                    invest.Post = post;

                    posts.Add(invest);
                }
            }

            return posts;
        }
        
        public void CustomMigration()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ConfigureOptions(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                var posts = ReadCsvManually(@"C:\Users\38093\Downloads\Пропозиції від укрінвесту - список проектів 16.06.tsv");
                context.InvestPosts.AddRangeAsync(posts);
                context.SaveChanges();
            }
        }
    }
}
