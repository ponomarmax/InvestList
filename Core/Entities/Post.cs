namespace Core.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public PostType PostType { get; set; }

        public string? CreatedById { get; set; }

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
        public  IEnumerable<ImageMetadata>? ImagesV2 { get; set; }

        public int Priority { get; set; }
    }
}