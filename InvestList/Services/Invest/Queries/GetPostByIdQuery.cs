using InvestList.Models.V2;
using MediatR;
using Radar.Application.Models;
using Radar.Domain;

namespace InvestList.Services.Invest.Queries;

public class GetInvestPostByIdQuery : IRequest<InvestPostDetailDto?>
{
    public string Slug { get; set; }
    public string Language { get; set; } = Defaults.LanguageUA;

}