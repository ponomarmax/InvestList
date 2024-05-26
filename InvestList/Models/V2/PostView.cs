using DataAccess.Models;
using InvestList.Models.Comment;
using InvestList.Models.News;

namespace InvestList.Models.V2
{
    public class PostView
    {
        public Guid Id { get; set; }
        public PostType PostType { get; set; }

        public string CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public string Title { get; set; }
        
        public string? TitleSeo { get; set; }

        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }

        public string? DescriptionSeo { get; set; }
        public string? ImageBase64 { get; set; }

        public ImageView Image { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<TagView> Tags { get; set; }
        
        public IEnumerable<CommentView>? Comments { get; set; }

        public IEnumerable<LinkView>? Links { get; set; }

        public IEnumerable<PostView>? SimilarInvests { get; set; }
        public IEnumerable<PostView>? SimilarNews { get; set; }
    }
}