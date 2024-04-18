using DataAccess.Models;

namespace WebApplication1.Models.News
{
    public class GetNewsViewModel
    {
        public Guid Id { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Title { get; set; }
       
        public string? Description { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string? ImageBase64 { get; set; }
    }
}
