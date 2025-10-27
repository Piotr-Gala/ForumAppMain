using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;
    public UsersController(IUserRepository users) => _users = users;

    // POST /Users
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto request)
    {
        // validation (empty fields) will also be done by the repo and throw ValidationException -> 400 by middleware
        var created = await _users.AddAsync(new User { Username = request.UserName, Password = request.Password });


        var dto = new UserDto { Id = created.Id, UserName = created.Username };
        return Created($"/Users/{dto.Id}", dto);
    }

    // GET /Users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        var u = await _users.GetSingleAsync(id); // 404 if doesn't exist (middleware)
        return Ok(new UserDto { Id = u.Id, UserName = u.Username });
    }

    // GET /Users?query=abc
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetMany([FromQuery] string? query)
    {
        var q = _users.GetManyAsync(); // IQueryable<User>

        if (!string.IsNullOrWhiteSpace(query))  // filtering by username
            q = q.Where(u => u.Username.Contains(query, StringComparison.OrdinalIgnoreCase));

        var result = q.Select(u => new UserDto { Id = u.Id, UserName = u.Username })
                      .ToList();

        return Ok(result);
    }

    // PUT /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto request)
    {
        var u = await _users.GetSingleAsync(id); // 404 if doesn't exist

        u.Username = request.UserName;
        if (!string.IsNullOrWhiteSpace(request.Password))
            u.Password = request.Password;

        await _users.UpdateAsync(u); // may throw DuplicateEntityException -> 409
        return Ok(new UserDto { Id = u.Id, UserName = u.Username });
    }

    // DELETE /Users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _users.DeleteAsync(id); // 404 if doesn't exist
        return NoContent();
    }
}
