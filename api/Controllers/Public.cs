using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicController(
    IPublicService service
) : ControllerBase
{
    private readonly IPublicService _service = service;

    private async Task<bool> TestDBAsync()
    {
        try
        {
            await _service.TestDBAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    [HttpGet("Health")]
    public async Task<IActionResult> Health()
    {
        var isProduction = !Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")!.Contains("Server=db");

        return this.DefaultOk(
            new { DB = User.Identity!.IsAuthenticated ? $"{(await TestDBAsync() ? "Active" : "Inactive")}" : "Unauthorized" },
            $"Environment: {(isProduction ? "production" : "development")}"
        );
    }
}