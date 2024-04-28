namespace InvestList.Models.Comment
{
    public class PostCommentRequest
    {
        public string Text { get; set; }
        public Guid InvestAdId { get; set; }
        public Guid UserId { get; set; }
    }
}