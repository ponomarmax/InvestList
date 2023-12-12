using DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InvestAd> InvestAds { get; set; }
        public DbSet<InvestAdExtraInfo> InvestAdExtraInfo { get; set; }
        
        public DbSet<InvestField> InvestFields { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<News> News { get; set; }

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

            modelBuilder.Entity<NewsToTags>()
                .HasOne(x => x.News)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.NewsId);

            //modelBuilder.Entity<NewsToTags>()
            //    .HasOne(x => x.Tag)
            //    .HasForeignKey(x => x.InvestFieldId);

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            // Seed your initial data here

            // Example: Seed a user
            modelBuilder.Entity<InvestField>().HasData(
                new InvestField { Id = Guid.NewGuid(), Title = "Фінанси" },
                new InvestField { Id = Guid.NewGuid(), Title = "Сільськогосподарська техніка" },
                new InvestField { Id = Guid.NewGuid(), Title = "Займи" },
                new InvestField { Id = Guid.NewGuid(), Title = "Лізинг Авто" },
                new InvestField { Id = Guid.NewGuid(), Title = "Кафе та ресторани" }
            );

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = Guid.NewGuid(), Name = "Шахраї" },
                new Tag { Id = Guid.NewGuid(), Name = "Цікавинка" },
                new Tag { Id = Guid.NewGuid(), Name = "Сенсація" }
            );

            // Add more seed data as needed
        }
    }
}