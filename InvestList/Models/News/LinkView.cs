namespace InvestList.Models.News
{
    public class LinkView
    {
        public string AnchorText { get; set; }
        public string Hyperlink { get; set; }
        public Guid NewsId { get; set; }
        public bool Follow { get; set; }
    }
    
    public class PostLinkView
    {
        public string AnchorText { get; set; }
        public string Hyperlink { get; set; }
        public bool? Follow { get; set; }
    }
}