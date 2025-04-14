namespace InvestList.Models.V2
{
    public class InvestPostDetailDto
    {
        public Radar.Application.Models.PostDetailDto Post { get; set; }
        public DateTime UpdateAt { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public IEnumerable<CurrencyView> MinInvestValues { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }
    }
}