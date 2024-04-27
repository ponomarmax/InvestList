using Common;

namespace InvestList.Models.Invest
{
    public class PostInvestAdViewModel
    {
        public string AuthorId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public IDictionary<Currency, decimal?>? AcceptedCurrencies { get; set; }

        public int? InvestDurationYears { get; set; }

        public int? InvestDurationMonths { get; set; }

        public decimal? TotalInvestment { get; set; }

        public ICollection<string>? InvestFields { get; set; }

        public string? ImageBase64 { get; set; }

        public bool Published { get; set; }
    }
}
