using InvestList.Models.Comment;
using InvestList.Models.Invest;

namespace InvestList.Models.News
{
    public class GetNewsViewModel
    {
        public Guid Id { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Title { get; set; }
       
        public string? Description { get; set; }
        
        public string? TitleSeo { get; set; }

        public string? DescriptionSeo { get; set; }

        public IEnumerable<TagView> Tags { get; set; }
        public IEnumerable<LinkView> Links { get; set; }
        public IEnumerable<CommentView> Comments { get; set; }
        public string? ImageBase64 { get; set; }
        public IEnumerable<GetAllAdsView> SimilarInvests { get; set; }
        public IEnumerable<GetNewsViewModel> SimilarNews { get; set; }
    }
}
