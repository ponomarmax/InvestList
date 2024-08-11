//it has removed code but just for history purpose stays here for a while
// using Core.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Migrations;
// using Microsoft.Extensions.Configuration;
//
// #nullable disable
//
// namespace DataAccess.Migrations
// {
//     /// <inheritdoc />
//     public partial class migratelinkstopost : Migration
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
//             optionsBuilder.EnableSensitiveDataLogging(true);
//
//             using (var context = new ApplicationDbContext(optionsBuilder.Options))
//             {
//                 var slugs = new HashSet<string>();
//
//                 var links = new List<PostLink>();
//                 foreach (var news in context.News.Include(x=>x.Links)
//                              .AsNoTracking())
//                 {
//                     foreach (var link in news.Links)
//                     {
//                         links.Add(new PostLink()
//                             {
//                                 AnchorText = link.AnchorText,
//                                 CreatedAt = link.CreatedAt.DateTime,
//                                 Follow = link.Follow,
//                                 Hyperlink = link.Hyperlink,
//                                 PostId = news.Id,
//                             }
//                             );
//                     }
//                 }
//
//                 context.PostLinks.AddRangeAsync(links);
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
//
//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//
//         }
//     }
// }
