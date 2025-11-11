using ApiContracts.Comments;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddAsync(CreateCommentDto request);
    public Task<List<CommentDto>> GetByPostIdAsync(int postId);
}

using ApiContracts.Posts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>> GetAllAsync();

    public Task<PostDto> GetByIdAsync(int id);

    // more methods...
}

using ApiContracts.Users;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<List<UserDto>> GetAllAsync();
    public Task UpdateUserAsync(int id, UpdateUserDto request);

    // more methods...
}