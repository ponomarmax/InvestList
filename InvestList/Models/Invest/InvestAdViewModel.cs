using DataAccess.Models;
using InvestList.Models.Comment;

namespace InvestList.Models.Invest
{
    public class InvestAdViewModel
    {
        public Guid Id { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        //public ICollection<InvestAdExtraInfo> History { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public ICollection<MinimalInvestEntrance> AcceptedCurrencies { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }

        public virtual ICollection<InvestFieldView> InvestFields { get; set; }

        public string? ImageBase64 { get; set; }
        public bool Published { get; set; }
        
        public List<CommentView> Comments { get; set; }
    }
}
