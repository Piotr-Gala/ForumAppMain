using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Shared.ApiContracts.Auth;
using ApiContracts.Users;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    public AuthController(IUserRepository users) => _users = users;

    // POST /Auth/Login
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserDto> Login([FromBody] LoginRequestDto request)
    {
        // 1) find by username
        var user = _users.GetManyAsync().FirstOrDefault(u => u.Username == request.Username);

        // 2) user not found -> 401
        if (user is null) return Unauthorized();

        // 3) bad password -> 401
        if (user.Password != request.Password) return Unauthorized();

        // 4) map to UserDto (no password)
        var dto = new UserDto { Id = user.Id, Username = user.Username };

        // 5) return UserDto
        return Ok(dto);
    }
}
