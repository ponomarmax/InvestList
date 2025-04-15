using InvestList.Models.V2;
using MediatR;
using Radar.Application.Models;

namespace InvestList.Services.Invest.Commands;

public class UpdateInvestPostCommand : IRequest<string>
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public PostDataDto Post { get; set; } = new();
    public InvestPostDto InvestPost { get; set; } = new();
}
