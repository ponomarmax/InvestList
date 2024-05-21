namespace DataAccess.Models
{
    public class PostComment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}