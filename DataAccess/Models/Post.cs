namespace DataAccess.Models
{
    public enum PostType{
        Undefined = 0,
        InvestAd = 1,
        News = 2,
    }
    
    public class Post
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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<PostTags>? Tags { get; set; }
        public IEnumerable<PostLink>? Links { get; set; }
        public IEnumerable<PostComment>? Comments { get; set; }
        public  IEnumerable<Image>? Images { get; set; }
    }
    
    public class PostTags
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
    
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageBase64 { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}