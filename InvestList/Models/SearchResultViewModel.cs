namespace InvestList.Models
{
    public class SearchResultViewModel
    {
        public Guid InvestId { get; set; }

        public string Title { get; set; }
        
        public string? Description { get; set; }

        public string? SpendInvestDesc { get; set; }

        public string? ProfitPaymentScheme { get; set; }

        public string? OtherInfo { get; set; }
    }
}
