using ApiContracts.Posts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>> GetAllAsync();

    public Task<PostDto> GetByIdAsync(int id);

    // NEW:
    Task<List<PostDto>> GetFilteredAsync(string? title = null, int? userId = null);
    Task UpdateAsync(int id, UpdatePostDto request);
    Task DeleteAsync(int id);
    
    // more methods...
}