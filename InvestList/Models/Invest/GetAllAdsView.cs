using Core.Entities;

namespace InvestList.Models.Invest
{
    public class GetAllAdsView
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<MinimalInvestEntrance> AcceptedCurrencies { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public IEnumerable<TagView> Tags { get; set; }
        // public string[] InvestFields { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid AuthorId { get; set; }

        public string Author { get; set; }

        public string? ImageData { get; set; }
    }
}
