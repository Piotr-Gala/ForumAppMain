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

    // POST 
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto request)
    {
        
        var created = await _users.AddAsync(new User { Username = request.Username, Password = request.Password });

        var dto = new UserDto { Id = created.Id, Username = created.Username };
        return Created($"/Users/{dto.Id}", dto);
    }

    // GET 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        var u = await _users.GetSingleAsync(id); 
        return Ok(new UserDto { Id = u.Id, Username = u.Username });
    }

    // GET 
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetMany([FromQuery] string? query)
    {
        var q = _users.GetManyAsync(); 

        if (!string.IsNullOrWhiteSpace(query))  
            q = q.Where(u => u.Username.Contains(query, StringComparison.OrdinalIgnoreCase));

        var result = q.Select(u => new UserDto { Id = u.Id, Username = u.Username })
                      .ToList();

        return Ok(result);
    }

    // PUT 
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto request)
    {
        var u = await _users.GetSingleAsync(id); 

        u.Username = request.Username;
        if (!string.IsNullOrWhiteSpace(request.Password))
            u.Password = request.Password;

        await _users.UpdateAsync(u); 
        return Ok(new UserDto { Id = u.Id, Username = u.Username });
    }

    // DELETE 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _users.DeleteAsync(id); 
        return NoContent();
    }
}
