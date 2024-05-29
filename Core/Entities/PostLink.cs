namespace Core.Entities
{
    public class PostLink
    {
        public Guid Id { get; set; }
        public string AnchorText { get; set; }
        public string Hyperlink { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public bool Follow { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}