namespace Core.Entities
{
    public class InvestPost
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public List<MinInvestValue> MinInvestValues { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }
    }
    
    public static class InvestPostManualMapper
    {
        public static void MapTo(this InvestPost source, InvestPost target)
        {
            if (source == null || target == null)
                return;

            target.InvestDurationYears = source.InvestDurationYears;
            target.InvestDurationMonths = source.InvestDurationMonths;
            target.TotalInvestment = source.TotalInvestment;
            target.AnnualInvestmentReturn = source.AnnualInvestmentReturn;

            // Якщо потрібно повністю замінити список мінімальних інвест значень:
            target.MinInvestValues = source.MinInvestValues != null
                ? source.MinInvestValues
                    .Select(mv => new MinInvestValue
                    {
                        Currency = mv.Currency,
                        MinValue = mv.MinValue
                    }).ToList()
                : new List<MinInvestValue>();
        }
    }
}