using Core;

namespace InvestList.Models.V2
{
    public class PutInvestModel
    {
        public decimal AnnualInvestmentReturn { get; set; }

        public List<CurrencyView>? MinInvestValues { get; set; } = [];

        public int? InvestDurationYears { get; set; }

        public int? InvestDurationMonths { get; set; }

        public decimal? TotalInvestment { get; set; }
    }

    public class CurrencyView
    {
        public Currency? Currency { get; set; }
        public decimal? MinValue { get; set; }
    }
}