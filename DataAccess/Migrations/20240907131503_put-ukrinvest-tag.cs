using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Radar.Domain.Entities;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class putukrinvesttag : Migration
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

        public void CustomMigration()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ConfigureOptions(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
var anchorDateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                foreach (var post in context.Posts.Include(x=>x.Tags))
                {
                    if (post.CreatedAt < anchorDateTime)
                    {
                        // if(post.Tags == null)
                        //     post.Tags = new PostTags<[]{new PostTags(){TagId = Guid.Parse("0C9B81E4-A577-4021-36F6-08DCCBE0B91D")}};
                        // else
                        // {
                        //     post.Tags = post.Tags.Union(new []{new PostTags(){TagId = Guid.Parse("0C9B81E4-A577-4021-36F6-08DCCBE0B91D")}}).ToList();
                        // }
                    }
                        
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
