namespace DataAccess.Models
{
    public record InvestField
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<InvestAdExtraInfoInvestField> InvestAdExtraInfos { get; set; }
    }
}
