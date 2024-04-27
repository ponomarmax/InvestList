namespace InvestList.Models.Comment
{
    public class CommentView
    {
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }

        public string Author { get; set; }
    }
}