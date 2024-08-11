///
/// it has removed code but just for history purpose stays here for a while
/// 

// using Core;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Migrations;
// using Microsoft.Extensions.Configuration;
//
// #nullable disable
//
// namespace DataAccess.Migrations
// {
//     /// <inheritdoc />
//     public partial class generate_slug() : Migration
//     {
//         private void ConfigureOptions(DbContextOptionsBuilder optionsBuilder)
//         {
//             // Load the connection string from appsettings.json
//             var basePath = AppDomain.CurrentDomain.BaseDirectory;
//             var configuration = new ConfigurationBuilder()
//                     .AddJsonFile("appsettings.json", false, true)
//                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
//                         true, true)
//                     .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true)
//                     .AddEnvironmentVariables().Build();
//
//             var connectionString = configuration.GetConnectionString("DefaultConnection");
//             optionsBuilder.UseSqlServer(connectionString);
//         }
//
//         public void CustomMigration()
//         {
//             var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//             ConfigureOptions(optionsBuilder);
//
//             using (var context = new ApplicationDbContext(optionsBuilder.Options))
//             {
//                 var slugs = new HashSet<string>();
//                 foreach (var news in context.News)
//                 {
//                     var slugCandidate = SlugGenerator.Get(news.Title);
//                     if (slugs.Add(slugCandidate))
//                     {
//                         news.Slug = slugCandidate;
//                     }
//                     else
//                     {
//                         news.Slug = $"{slugCandidate}-{Guid.NewGuid().ToString()[..7]}";
//                     }
//                 }
//                 slugs = new HashSet<string>();
//                 foreach (var investAd in context.InvestAds)
//                 {
//                     var history = context.InvestAdExtraInfo.Where(x => x.InvestAdId == investAd.Id)
//                         .OrderBy(x => x.CreatedAt).Last();
//                     var slugCandidate = SlugGenerator.Get(history.Title);
//                     if (slugs.Add(slugCandidate))
//                     {
//                         investAd.Slug = slugCandidate;
//                     }
//                     else
//                     {
//                         investAd.Slug = $"{slugCandidate}-{Guid.NewGuid().ToString()[..7]}";
//                     }
//                 }
//
//                 context.SaveChanges();
//             }
//         }
//         
//         /// <inheritdoc />
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             CustomMigration();
//         }
//
//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             
//         }
//     }
// }
