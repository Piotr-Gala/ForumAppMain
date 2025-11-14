using Microsoft.AspNetCore.Mvc;
using WebAPI.RepositoryContracts;
using Shared.ApiContracts.Auth;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    // POST /Auth/Login
    [HttpPost("Login")]
    public ActionResult<string> Login([FromBody] LoginRequestDto request)
    {
        // Dummy authentication logic for demonstration purposes
        if (request.Username == "admin" && request.Password == "password")
        {
            // In a real application, generate a JWT or similar token here
            return Ok("dummy-auth-token");
        }
        return Unauthorized();
    }
}