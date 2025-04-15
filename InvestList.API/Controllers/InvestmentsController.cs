using InvestList.Services.Invest.Commands;
using InvestList.Services.Invest.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Radar.Application.Posts.Commands;

namespace InvestList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvestmentsController : BaseController
{
    private readonly IMediator _mediator;

    public InvestmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvestments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string language = "uk",
        [FromQuery] List<Guid>? tagIds = null)
    {
        var query = new GetInvestPostListQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            Language = language,
            TagIds = tagIds ?? new List<Guid>()
        };

        var result = await _mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetInvestment(string slug, [FromQuery] string language = "uk")
    {
        var query = new GetInvestPostByIdQuery
        {
            Slug = slug,
            Language = language
        };

        var result = await _mediator.Send(query);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvestment([FromBody] CreateInvestPostCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetInvestment), new { slug = result });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvestment(Guid id, [FromBody] UpdatePostCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    // [HttpGet("count")]
    // public async Task<IActionResult> GetInvestmentsCount()
    // {
    //     var query = new GetInvestmentsCountQuery();
    //     var result = await _mediator.Send(query);
    //     return Ok(new { count = result });
    // }
} 