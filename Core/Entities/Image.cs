namespace Core.Entities
{
    [Obsolete]
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageBase64 { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}