using DataAccess.Models;

namespace InvestList.Models.Comment
{
    public class PostCommentRequest
    {
        public string Text { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public PostType PostType { get; set; }
    }
}