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

    // POST 
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto request)
    {
        
        var created = await _comments.AddAsync(new Comment { Body = request.Body, UserId = request.UserId, PostId = request.PostId });

        var dto = new CommentDto { Id = created.Id, Body = created.Body, UserId = created.UserId, PostId = created.PostId };
        return Created($"/Comments/{dto.Id}", dto);
    }

    // GET 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        var c = await _comments.GetSingleAsync(id); 
        return Ok(new CommentDto { Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId });
    }

    // GET 
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany(
        [FromQuery] int? postId,
        [FromQuery] int? userId)
    {
        var q = _comments.GetManyAsync(); 

        
        if (postId is not null)  
            q = q.Where(c => c.PostId == postId.Value);

        if (userId is not null)  
            q = q.Where(c => c.UserId == userId.Value);

        var result = q.Select(c => new CommentDto { Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId })
                      .ToList();

        return Ok(result);
    }

    // PUT 
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> Update(int id, [FromBody] UpdateCommentDto request)
    {
        var c = await _comments.GetSingleAsync(id); 

        c.Body = request.Body;
 
        await _comments.UpdateAsync(c); 
        return Ok(new CommentDto { Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId });
    }

            

    // DELETE 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _comments.DeleteAsync(id); 
        return NoContent();
    }
}
