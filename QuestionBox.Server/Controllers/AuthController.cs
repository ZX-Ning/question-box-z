using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace QuestionBox.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    ILogger<AuthController> logger,
    LoginChecker loginChecker
) : ControllerBase {
    public record LoginRequest(string name, string password);
    string scheme = CookieAuthenticationDefaults.AuthenticationScheme;
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) {
        if (!loginChecker.check(loginRequest.name, loginRequest.password)) {
            return Unauthorized(new { Message = "Incorrect credential" });
        }
        // Authenticated:
        List<Claim> claims = [
            new (ClaimTypes.Name, "loginRequest.name"),
            new (ClaimTypes.Role, "Admin")
        ];
        ClaimsIdentity identity = new(claims, scheme);
        AuthenticationProperties properties = new() {
            IsPersistent = true,
        };
        await HttpContext.SignInAsync(scheme, new ClaimsPrincipal(identity), properties);
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