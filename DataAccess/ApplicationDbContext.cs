using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): IdentityDbContext<User>(options)
    {
        public DbSet<InvestAd> InvestAds { get; set; }
        public DbSet<InvestAdExtraInfo> InvestAdExtraInfo { get; set; }

        public DbSet<InvestField> InvestFields { get; set; }
        public DbSet<Link> Links { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CustomHeader> CustomHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InvestAdExtraInfoInvestField>()
                .HasKey(x => new { x.InvestAdExtraInfoId, x.InvestFieldId });

            modelBuilder.Entity<InvestAdExtraInfoInvestField>()
                .HasOne(x => x.InvestAdExtraInfo)
                .WithMany(x => x.InvestFields)
                .HasForeignKey(x => x.InvestAdExtraInfoId);

            modelBuilder.Entity<InvestAdExtraInfoInvestField>()
                .HasOne(x => x.InvestField)
                .WithMany(x => x.InvestAdExtraInfos)
                .HasForeignKey(x => x.InvestFieldId);

            modelBuilder.Entity<NewsToTags>()
                .HasKey(x => new { x.NewsId, x.TagId });
            
            modelBuilder.Entity<InvestTags>()
                .HasKey(x => new { x.InvestId, x.TagId });

            modelBuilder.Entity<NewsToTags>()
                .HasOne(x => x.News)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.NewsId);
            
            modelBuilder.Entity<InvestTags>()
                .HasOne(x => x.Invest)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.InvestId);
            
            modelBuilder.Entity<Comment>()
                .HasOne(x => x.InvestAd)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.InvestAdId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InvestAd>()
                .HasIndex(e => e.Slug);
            modelBuilder.Entity<News>()
                .HasIndex(e => e.Slug);
            
            var postTypeConverter = new ValueConverter<PostType, string>(
                v => v.ToString(),
                v => (PostType)Enum.Parse(typeof(PostType), v));
            modelBuilder.Entity<Post>()
                .Property(p => p.PostType)
                .HasConversion(postTypeConverter);
            modelBuilder.Entity<PostTags>()
                .HasKey(x => new { x.PostId, x.TagId });
            modelBuilder.Entity<PostTags>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.PostId)
                .HasForeignKey(x=>x.TagId);
            
            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestField>().HasData(
                new InvestField { Id = Guid.Parse("4F711134-52BB-4BBF-A2D2-E59EDA732D67"), Title = "Фінанси" },
                new InvestField
                    { Id = Guid.Parse("92EE46D1-5461-4772-8DBD-0EF62A8A1D34"), Title = "Сільськогосподарська техніка" },
                new InvestField { Id = Guid.Parse("42C6356D-6754-434E-AB92-99A6FDFD1D88"), Title = "Займи" },
                new InvestField
                    { Id = Guid.Parse("DA5EAB20-13AA-4F44-A30B-7BE764DBCFBF"), Title = "Кафе та ресторани" },
                new InvestField
                    { Id = Guid.Parse("4AC89C0C-B3DE-488F-A99B-42601585B9AC"), Title = "Нерухомість в Україні" },
                new InvestField
                    { Id = Guid.Parse("9850F830-79CB-4E4C-8091-FA577047377D"), Title = "Нерухомість закордоном" },
                new InvestField { Id = Guid.Parse("BF6A5DE8-1BBC-4367-9812-58EB3D1D7834"), Title = "Агро" },
                new InvestField { Id = Guid.Parse("8F4DE586-06D7-45ED-BBBE-D6F732B02337"), Title = "IT" },
                new InvestField { Id = Guid.Parse("6B38FCF2-47C5-41F2-878C-3A7C37072C55"), Title = "Рітейл" },
                new InvestField { Id = Guid.Parse("07BA9B0E-ADED-4706-AE0D-820D10CD2A7F"), Title = "Авто" },
                new InvestField { Id = Guid.Parse("7E3FA98E-9422-468D-8DCD-D66673583A76"), Title = "Криптовалюти" },
                new InvestField { Id = Guid.Parse("9D1C74FB-4160-46C7-BC4D-153A23EF39A3"), Title = "Виробництво" },
                new InvestField { Id = Guid.Parse("E9DC75B6-DF9E-456F-9CDE-0A4435391C90"), Title = "Розваги" },
                new InvestField { Id = Guid.Parse("60BDBCEC-5716-4593-B3EA-3F4C080F03E4"), Title = "Енергетика" },
                new InvestField { Id = Guid.Parse("3AD6CD6E-FC51-4F55-A20B-304E208667B9"), Title = "Освіта" },
                new InvestField { Id = Guid.Parse("072AA2C8-7641-4E36-AF85-41D0CEF7DB4D"), Title = "Спорт" }
            );

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