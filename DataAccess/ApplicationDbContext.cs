using Common;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Radar.Domain.Entities;
using Post = Radar.Domain.Entities.Post;

namespace DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<PostLink> PostLinks { get; set; }
        public DbSet<GoogleAnalyticPostView> GoogleAnalyticPostViews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<InvestPost> InvestPosts { get; set; }
        public DbSet<MinInvestValue> MinInvestValue { get; set; }
        public DbSet<CustomHeader> CustomHeaders { get; set; }
        public DbSet<ImageMetadata> ImageMetadata { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<SeoDetails> SeoDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Post>()
                .ToTable("Posts")
                .HasOne(p => p.CreatedBy)
                .WithMany() // No navigation property
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);
            // modelBuilder.Entity<Post>()
            //     .HasOne(x => x.CreatedBy).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasIndex(e => e.Slug);

            modelBuilder.Entity<SeoDetails>()
                .HasIndex(e => e.RelativePagePath);

            modelBuilder.Entity<Post>()
                .HasIndex(e => new { e.PostType, e.IsActive });
            modelBuilder.Entity<Post>()
                .HasIndex(e => e.CreatedAt);

            // var postTypeConverter = new ValueConverter<PostType, string>(
            //     v => v.ToString(),
            //     v => (PostType)Enum.Parse(typeof(PostType), v));
            var currencyConverter = new ValueConverter<Currency, string>(
                v => v.ToString(),
                v => (Currency)Enum.Parse(typeof(Currency), v));
            // modelBuilder.Entity<Post>()
            //     .Property(p => p.PostType)
            //     .HasConversion(postTypeConverter);
            modelBuilder.Entity<MinInvestValue>()
                .Property(p => p.Currency)
                .HasConversion(currencyConverter);
            
            modelBuilder.Entity<MinInvestValue>()
                .HasOne(p => p.InvestPost)
                .WithMany(x=>x.MinInvestValues)
                .HasForeignKey(p => p.InvestPostId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<PostTags>()
                .HasKey(x => new { x.PostId, x.TagId });
            modelBuilder.Entity<PostTags>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<PostTags>()
                .HasOne(x => x.Tag).WithMany(x => x.Posts).HasForeignKey(x => x.TagId);
            
            modelBuilder.Entity<TagTranslation>()
                .HasOne(x => x.Tag).WithMany(x => x.Translations).HasForeignKey(x => x.TagId);
            
            modelBuilder.Entity<PostTranslation>()
                .HasOne(x=>x.Post)
                .WithMany(x=>x.Translations)
                .HasForeignKey(x=>x.PostId);
            
            modelBuilder.Entity<PostComment>()
                .HasOne(x=>x.Post)
                .WithMany(x=>x.Comments)
                .HasForeignKey(x=>x.PostId);
            
            modelBuilder.Entity<PostLink>()
                .HasOne(x=>x.Post)
                .WithMany(x=>x.Links)
                .HasForeignKey(x=>x.PostId);
            
            modelBuilder.Entity<GoogleAnalyticPostView>()
                .HasOne(x=>x.Post)
                .WithOne(x=>x.GoogleAnalyticPostView);
            modelBuilder.Entity<TopPostWithInvestResult>().HasNoKey();

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
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