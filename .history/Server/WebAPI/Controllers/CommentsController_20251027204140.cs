using ApiContracts.Comments;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _comments;
    public CommentsController(ICommentRepository comments) => _comments = comments;

    // POST /Comments
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto request)
    {
        // validation (empty fields) will also be done by the repo and throw ValidationException -> 400 by middleware
        var created = await _comments.AddAsync(new Comment { Body = request.Body, UserId = request.UserId, PostId = request.PostId });

        var dto = new CommentDto { Id = created.Id, Body = created.Body, UserId = created.UserId, PostId = created.PostId };
        return Created($"/Comments/{dto.Id}", dto);
    }

    // GET /Comments/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        var c = await _comments.GetSingleAsync(id); // 404 if doesn't exist (middleware)
        return Ok(new CommentDto { Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId });
    }

    // GET /Comments?query=abc
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany([FromQuery] string? query)
    {
        var q = _comments.GetManyAsync(); // IQueryable<Comment>

        if (!string.IsNullOrWhiteSpace(query))  // filtering by body
            q = q.Where(c => c.Body.Contains(query, StringComparison.OrdinalIgnoreCase));

        var result = q.Select(c => new CommentDto { Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId })
                      .ToList();

        return Ok(result);
    }

    // PUT /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> Update(int id, [FromBody] UpdateCommentDto request)
    {
        var c = await _comments.GetSingleAsync(id); // 404 if doesn't exist

        c.Body = request.Body;
 
        await _comments.UpdateAsync(c); // may throw DuplicateEntityException -> 409
        return Ok(new CommentDto { Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId });
    }

            

    // DELETE /Users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _comments.DeleteAsync(id); // 404 if doesn't exist
        return NoContent();
    }
}
