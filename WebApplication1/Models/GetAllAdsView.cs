using DataAccess.Models;

namespace WebApplication1.Models
{
    public class GetAllAdsView
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ICollection<MinimalInvestEntrance> AcceptedCurrencies { get; set; }

        public TimeSpan InvestPeriod { get; set; }

        public string[] InvestFields { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid AuthorId { get; set; }

        public string Author { get; set; }
    }
}
