using DataAccess.Models;

namespace InvestList.Models.News
{
    public class PostNewsViewModel
    {
        public string AuthorId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public ICollection<string>? Tags { get; set; }

        public IEnumerable<PostLinkView> Links { get; set; }

        public string? ImageBase64 { get; set; }
    }
}
