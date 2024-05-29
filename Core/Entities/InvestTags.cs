namespace Core.Entities
{
    [Obsolete]
    public class InvestTags
    {
        public Guid InvestId { get; set; }
        public InvestAd Invest { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
