using DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class InvestAdViewModel
    {
        public Guid Id { get; set; }

        public string AuthorId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        //public ICollection<InvestAdExtraInfo> History { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public string? SpendInvestDesc { get; set; }

        public string? ProfitPaymentScheme { get; set; }

        public string? OtherInfo { get; set; }

        public ICollection<MinimalInvestEntrance> AcceptedCurrencies { get; set; }

        public int InvestDurationYears { get; set; }

        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }

        public virtual ICollection<InvestFieldView> InvestFields { get; set; }
    }
}
