using Microsoft.AspNetCore.Mvc;

namespace InvestList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(T result)
    {
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    protected IActionResult HandleResult<T>(IEnumerable<T> results)
    {
        if (results == null || !results.Any())
            return NotFound();
        
        return Ok(results);
    }

    protected IActionResult HandleCreatedResult<T>(T result, string actionName, object routeValues)
    {
        return CreatedAtAction(actionName, routeValues, result);
    }
} 