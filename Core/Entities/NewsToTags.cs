namespace Core.Entities
{
    [Obsolete]
    public class NewsToTags
    {
        public Guid NewsId { get; set; }
        public News News { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
