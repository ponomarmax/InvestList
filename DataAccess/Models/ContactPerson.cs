using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class ContactPerson
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [ForeignKey("RelatedInvestAd")]
        public Guid RelatedInvestAdId { get; set; }
        
        public virtual InvestAdExtraInfo RelatedInvestAd { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneVerified { get; set; }
    }
}
