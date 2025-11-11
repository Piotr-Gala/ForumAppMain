using ApiContracts.Posts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _posts;
    public PostsController(IPostRepository posts) => _posts = posts;

    // POST 
    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostDto request)
    {
       
        var created = await _posts.AddAsync(new Post { Title = request.Title, Body = request.Body, UserId = request.UserId });

        var dto = new PostDto { Id = created.Id, Title = created.Title, Body = created.Body, UserId = created.UserId };
        return Created($"/Posts/{dto.Id}", dto);
    }

    // GET 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id)
    {
        var p = await _posts.GetSingleAsync(id); 
        return Ok(new PostDto { Id = p.Id, Title = p.Title, Body = p.Body, UserId = p.UserId });
    }

    // GET 
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetMany(
        [FromQuery] string? title,
        [FromQuery] int? userId)
    {
        var q = _posts.GetManyAsync(); 

        if (!string.IsNullOrWhiteSpace(title))  
            q = q.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        if (userId is not null) 
            q = q.Where(p => p.UserId == userId.Value);

        var result = q.Select(p => new PostDto { Id = p.Id, Title = p.Title, Body = p.Body, UserId = p.UserId })
                      .ToList();

        return Ok(result);
    }

    // PUT 
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostDto>> Update(int id, [FromBody] UpdatePostDto request)
    {
        var p = await _posts.GetSingleAsync(id); 

        p.Title = request.Title;
        p.Body = request.Body;

        await _posts.UpdateAsync(p); 
        return Ok(new PostDto { Id = p.Id, Title = p.Title, Body = p.Body, UserId = p.UserId });
    }

            

    // DELETE 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _posts.DeleteAsync(id);
        return NoContent();
    }
}
