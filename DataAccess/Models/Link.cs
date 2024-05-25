using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Link
    {
        [Key]
        public Guid Id { get; set; }
        public string AnchorText { get; set; }
        public string Hyperlink { get; set; }
        [ForeignKey("News")]
        public Guid NewsId { get; set; }
        public virtual News News { get; set; }
        public bool Follow { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
    
    public class PostLink
    {
        public Guid Id { get; set; }
        public string AnchorText { get; set; }
        public string Hyperlink { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public bool Follow { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}