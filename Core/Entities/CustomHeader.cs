namespace Core.Entities
{
    public class CustomHeader
    {
        public Guid Id { get; set; }
        public Tag Tag { get; set; }
        public Guid TagId { get; set; }
    }
}