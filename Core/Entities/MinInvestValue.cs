namespace Core.Entities
{
    public record MinInvestValue
    {
        public Guid Id { get; set; }
        public decimal MinValue { get; set; }
        public Guid InvestPostId { get; set; }
        public InvestPost InvestPost { get; set; }
        public Currency Currency { get; set; }
    }
}