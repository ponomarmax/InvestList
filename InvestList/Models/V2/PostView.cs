using DataAccess.Models;

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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<TagView> Tags { get; set; }
    }
}