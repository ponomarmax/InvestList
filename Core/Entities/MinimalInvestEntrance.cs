namespace Core.Entities
{
    [Obsolete]
    public record MinimalInvestEntrance
    {
        public Guid Id { get; set; }
        public decimal MinValue { get; set; }
        public Currency Currency { get; set; }
    }
}
