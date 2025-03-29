using Common;

namespace InvestList.Models.V2
{
    public class PutPostModel
    {
        public string? Title { get; set; }
        public string? TitleSeo { get; set; }

        public string? Description { get; set; }
        public string? DescriptionSeo { get; set; }

        public IEnumerable<string>? TagIds { get; set; }

        public string? ImageBase64 { get; set; }

        public bool IsActive { get; set; }
        
        public IEnumerable<PostLinkView>? Links { get; set; }
        
        public List<PutPostTranslationModel> Translations { get; set; } = new();
    }
    
    public class PutPostTranslationModel
    {
        public string Language { get; set; } = "uk"; // або "en", "pl" тощо
        public string? Title { get; set; }
        public string? TitleSeo { get; set; }
        public string? Description { get; set; }
        public string? DescriptionSeo { get; set; }
    }
}
