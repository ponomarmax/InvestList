namespace Core.Entities;

public class GoogleAnalyticPostView
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public int PageViews { get; set; }
    public DateTime LastUpdated { get; set; }
}