using InvestList.Models.V2;
using MediatR;

namespace InvestList.Services.Invest.Queries;

public class GetInvestPostWithSimilarQuery(string slug, string language) : IRequest<InvestPostWithSimilarResponseDto>
{
    public string Slug { get; set; } = slug;
    public string Language { get; set; } = language;
}