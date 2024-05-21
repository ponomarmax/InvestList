namespace DataAccess.Models
{
    public class InvestPost
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public IEnumerable<MinimalInvestEntrance> AcceptedCurrencies { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }
    }
}