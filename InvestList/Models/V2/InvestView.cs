using InvestList.Models.Comment;

namespace InvestList.Models.V2
{
    public class InvestView : PostView
    {
        public DateTime UpdateAt { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public IEnumerable<CurrencyView> MinInvestValues { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }

        public string? ImageBase64 { get; set; }
        
        public IEnumerable<CommentView> Comments { get; set; }
        public IEnumerable<PostView> SimilarInvests { get; set; }
        public IEnumerable<PostView> SimilarNews { get; set; }
    }
}