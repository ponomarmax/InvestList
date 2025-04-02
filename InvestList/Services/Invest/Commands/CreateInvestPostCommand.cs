using InvestList.Models.V2;
using MediatR;
using Radar.Application.Models;
using Radar.Application.Posts.Commands;

namespace InvestList.Services.Invest.Commands;

public class CreateInvestPostCommand: IRequest<Guid>
{
    public string UserId { get; set; } = string.Empty;
    public PostDataDto Post { get; set; } = new();
    public InvestPostDto InvestPost { get; set; }
}