using System.Globalization;
using InvestList.Services.Queries;
using MediatR;
using Radar.Application.Models;

namespace InvestList.Services.Invest.Queries;

public class GetInvestPostListQuery : IRequest<PaginatedResult<InvestPostShortDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string Language { get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    public List<Guid> TagIds { get; set; } = new();
}