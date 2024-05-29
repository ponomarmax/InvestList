namespace Core.Entities
{
    public class ImageMetadata
    {
        public Guid Id { get; set; }
        public Guid ImageObjectId { get; set; }
        public ImageObject ImageObject { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}