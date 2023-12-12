using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class News
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public ICollection<NewsToTags> Tags { get; set; }

        public string? ImageBase64 { get; set; }
    }
}
