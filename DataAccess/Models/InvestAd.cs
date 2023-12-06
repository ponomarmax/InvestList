using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class InvestAd
    {
        [Key]
        public Guid Id { get; set; }
        
        public bool Published { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        
        public virtual User Author { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdateAt { get; set; }

        public virtual ICollection<InvestAdExtraInfo> History { get; set; }
        //public virtual ICollection<Comment> Comments { get; set; }
    }
}
