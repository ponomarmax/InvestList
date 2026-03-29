using System.Globalization;
using InvestList.Models.V2;
using MediatR;
using Radar.Application.Models;

namespace InvestList.Services.Queries;

public class GetPrimarySectionsQuery : IRequest<PrimarySectionsDto>
{
    public string? Search { get; set; }
    public string Language { get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    public List<Guid> TagIds { get; set; } = new();
}

public class PrimarySectionsDto
{
    public List<PostShortDto> News { get; set; }
    public List<PostShortDto> Blacklists { get; set; }
    public List<InvestPostShortDto> Invests { get; set; }
    public List<PostShortWithCommentDto> PostWithComments { get; set; }
}

public class InvestPostShortDto
{
    public PostShortDto Post { get; set; }

    public IEnumerable<CurrencyView> MinInvestValues { get; set; }

    public int InvestDurationYears { get; set; }

    public int InvestDurationMonths { get; set; }

    public decimal TotalInvestment { get; set; }
    public decimal AnnualInvestmentReturn { get; set; }
}