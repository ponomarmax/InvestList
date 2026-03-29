using Microsoft.EntityFrameworkCore;

namespace Core.Entities;
[Keyless]
public class TopPostWithInvestResult
{
    public Guid PostId { get; set; }
    public string Slug { get; set; }
    public string PostType { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? PageViews { get; set; }
    public bool IsActive { get; set; }

    public Guid? InvestId { get; set; }
    public Guid? InvestPostId { get; set; }
    public decimal? TotalInvestment { get; set; }
    public int? InvestDurationYears { get; set; }
    public int? InvestDurationMonths { get; set; }
    public decimal? AnnualInvestmentReturn { get; set; }
    public decimal? MinValue { get; set; }
    public string? Currency { get; set; }

    public string? TranslationTitle { get; set; }
    public string? TranslationDescription { get; set; }

    public Guid? ImageId { get; set; }
    public Guid? TagId { get; set; }
    public string? TagTitle { get; set; }
}