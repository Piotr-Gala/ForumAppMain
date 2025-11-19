using ApiContracts.Comments;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddAsync(CreateCommentDto request);
    public Task<List<CommentDto>> GetByPostIdAsync(int postId);

    // NEW:
    Task<List<CommentDto>> GetByUserIdAsync(int userId);

    // more methods...
}