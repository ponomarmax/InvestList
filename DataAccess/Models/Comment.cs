using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string Text { get; set; }
        [ForeignKey("InvestAd")]
        public Guid InvestAdId { get; set; }
        public virtual InvestAd InvestAd { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}