using Common;

namespace DataAccess.Models
{
    public record MinInvestValue
    {
        public Guid Id { get; set; }
        public decimal MinValue { get; set; }
        public Currency Currency { get; set; }
    }
}