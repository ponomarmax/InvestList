using Radar.Domain.Entities;

namespace Core.Entities
{
    public class Post : BasePost
    {
        public string Title { get; set; }
        
        public string? TitleSeo { get; set; }

       
        public string? Description { get; set; }
        
        public string? DescriptionSeo { get; set; }

        // public GoogleAnalyticPostView GoogleAnalyticPostView { get; set; }
    }
}