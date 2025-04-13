using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Radar.Domain;
using Radar.Domain.Entities;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migratetranslation : Migration
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

             var connectionString = configuration.GetSection("InvestRadar:ConnectionStrings")
                 .GetValue<string>("DefaultConnection");
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
                     post.Translations = new List<PostTranslation>(){new PostTranslation()
                     {
                         Title = post.Title,
                         Description = post.Description,
                         TitleSeo = post.TitleSeo,
                         DescriptionSeo = post.DescriptionSeo,
                         Language = Defaults.Language
                     }};
                 }
                 
                 foreach (var tag in context.Tags)
                 {
                     tag.Translations = new List<TagTranslation>(){new TagTranslation()
                         {
                             Name = tag.Name,
                             Language = Defaults.LanguageUA
                         },
                         new TagTranslation()
                         {
                             Name = tag.Name,
                             Language = Defaults.LanguageEN
                         }
                     };
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
