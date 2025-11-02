using ApiContracts.Comments;

public interface ICommentService
{
    public Task<CommentDto> AddAsync(CreateCommentDto request);
    public Task<List<CommentDto>> GetByPostIdAsync(int postId);
}