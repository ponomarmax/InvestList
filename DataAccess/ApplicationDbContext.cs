using Common;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): IdentityDbContext<User>(options)
    {
        public DbSet<PostLink> PostLinks { get; set; }
        public DbSet<GoogleAnalyticPostView> GoogleAnalyticPostViews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<InvestPost> InvestPosts { get; set; }
        public DbSet<CustomHeader> CustomHeaders { get; set; }
        public DbSet<ImageMetadata> ImageMetadata { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.CreatedBy)
                .WithMany() // No navigation property
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);
            // modelBuilder.Entity<Post>()
            //     .HasOne(x => x.CreatedBy).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasIndex(e => e.Slug);
            modelBuilder.Entity<Post>()
                .HasIndex(e => new { e.PostType, e.IsActive });
            modelBuilder.Entity<Post>()
                .HasIndex(e => e.CreatedAt);

            var postTypeConverter = new ValueConverter<PostType, string>(
                v => v.ToString(),
                v => (PostType)Enum.Parse(typeof(PostType), v));
            var currencyConverter = new ValueConverter<Currency, string>(
                v => v.ToString(),
                v => (Currency)Enum.Parse(typeof(Currency), v));
            modelBuilder.Entity<Post>()
                .Property(p => p.PostType)
                .HasConversion(postTypeConverter);
            modelBuilder.Entity<MinInvestValue>()
                .Property(p => p.Currency)
                .HasConversion(currencyConverter);
            modelBuilder.Entity<PostTags>()
                .HasKey(x => new { x.PostId, x.TagId });
            modelBuilder.Entity<PostTags>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Tags);

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = Guid.Parse("6A48934D-6459-4AAA-806A-594B4F05C7C3"), Name = "SCUM" },
                new Tag { Id = Guid.Parse("A4A1BB92-EB6A-4538-AE0D-DFCF70D0528C"), Name = "Держрегулювання" },
                new Tag { Id = Guid.Parse("D12CB617-608E-4109-85CB-AF9F2CB95B6F"), Name = "Рейтинги" },
                new Tag { Id = Guid.Parse("681E4BC1-DE4E-42A7-98F6-4425476EFB03"), Name = "Інше" }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "6d13c839-26bb-4358-b895-aad006aa6134",
                    ConcurrencyStamp = "42c6e015-f7b3-4822-a377-1012db3002d5",
                    Name = "business", NormalizedName = "BUSINESS"
                },
                new IdentityRole()
                {
                    Id = "5f6217f5-00f5-45fd-b2ae-3b02ca7b6b62",
                    ConcurrencyStamp = "08b9f7df-baf1-4834-8b37-f81e487b49ab", Name = "admin", NormalizedName = "ADMIN"
                }
            );
        }
    }
}