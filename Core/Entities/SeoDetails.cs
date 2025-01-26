namespace Core.Entities;

public class SeoDetails
{
    public Guid Id { get; set; }
    public string RelativePagePath { get; set; }
    public string? PageTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? PageH1 { get; set; }
    public DateTime CreatedAt { get; set; }
}