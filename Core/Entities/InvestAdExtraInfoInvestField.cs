namespace Core.Entities
{
    [Obsolete]
    public class InvestAdExtraInfoInvestField
    {
        public Guid InvestAdExtraInfoId { get; set; }
        public InvestAdExtraInfo InvestAdExtraInfo { get; set; }

        public Guid InvestFieldId { get; set; }
        public InvestField InvestField { get; set; }
    }
}