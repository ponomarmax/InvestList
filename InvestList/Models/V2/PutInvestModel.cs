using Common;

namespace InvestList.Models.V2
{
    public class PutInvestModel
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public IEnumerable<CurrencyView>? MinInvestValues { get; set; }

        public int? InvestDurationYears { get; set; }

        public int? InvestDurationMonths { get; set; }

        public decimal? TotalInvestment { get; set; }

        public IEnumerable<string>? TagIds { get; set; }

        public string? ImageBase64 { get; set; }

        public bool IsActive { get; set; }
    }

    public class CurrencyView
    {
        public Currency? Currency { get; set; }
        public decimal? MinValue { get; set; }
    }
}
