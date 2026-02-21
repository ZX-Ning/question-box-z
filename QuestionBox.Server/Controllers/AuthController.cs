using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using QuestionBox.Auth;

namespace QuestionBox.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    ILogger<AuthController> logger,
    ILoginChecker loginChecker
) : ControllerBase {
    public record struct LoginRequest(string name, string password);
    string scheme = CookieAuthenticationDefaults.AuthenticationScheme;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) {
        var role = loginChecker.check(loginRequest.name, loginRequest.password);
        if (role != Role.Admin) {
            return Unauthorized(new { Message = "Incorrect credential" });
        }
        var roleName = Enum.GetName(role)!;

        // Authenticated:
        ClaimsIdentity identity = new(
            [
                new (ClaimTypes.Name, loginRequest.name),
                new (ClaimTypes.Role, roleName)
            ],
            scheme
        );
        await HttpContext.SignInAsync(
            scheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true }
        );
        logger.LogInformation($"User: {loginRequest.name} logged in");
        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await HttpContext.SignOutAsync(scheme);
        return Ok();
    }

    public record StatusDto(bool isLoggin);
    [HttpGet("status")]
    public StatusDto status() {
        if (User.Identity is not null && User.Identity.IsAuthenticated) {
            return new(isLoggin: true);
        }
        return new(isLoggin: false);
    }
}