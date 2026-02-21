using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace QuestionBox.Controllers;

[ApiController]
[Route("/api/config")]
public sealed class ConfigController(
    ILogger<ConfigController> logger,
    IConfiguration config
) : ControllerBase {
    public record struct ConfigDto(int questionLengthLimit, string name, string avatar, string description);

    [HttpGet]
    public ActionResult<ConfigDto> OnGet() {
        var c = config.GetSection("frontend").Get<ConfigDto>();
        logger.LogDebug($"Get Config: {c}");
        return Ok(c);
    }
}