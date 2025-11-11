using ApiContracts.Posts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>> GetAllAsync();

    public Task<PostDto> GetByIdAsync(int id);

    // more methods...
}