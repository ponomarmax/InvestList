using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class clenaupdata : Migration
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

                foreach (var post in context.Posts)
                {
                    // Add your logic about cleaning data here
                    // like post.Description = ... and your logic
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
