using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class InvestAdExtraInfo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        
        public virtual ICollection<MinimalInvestEntrance> AcceptedCurrencies { get; set; }

        public int InvestDurationYears { get; set; }
        
        public int InvestDurationMonths { get; set; }

        public decimal TotalInvestment { get; set; }

        public decimal AnnualInvestmentReturn { get; set; }

        public virtual ICollection<InvestAdExtraInfoInvestField> InvestFields { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        //[ForeignKey("ContactPerson")]
        //public Guid ContactPersonId { get; set; }
        //public virtual ContactPerson ContactPerson { get; set; }


        [ForeignKey("InvestAd")]
        public Guid InvestAdId { get; set; }
        
        public virtual InvestAd InvestAd { get; set; }

        public string? ImageBase64 { get; set; }
    }
}
