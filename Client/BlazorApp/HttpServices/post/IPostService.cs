using ApiContracts.Posts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>> GetAllAsync();

    public Task<PostDto> GetByIdAsync(int id);

    // NEW:
    Task<List<PostDto>> GetByTitleAsync(string title);
    Task<List<PostDto>> GetByUserIdAsync(int userId);
    Task UpdateAsync(int id, UpdatePostDto request);
    Task DeleteAsync(int id);
    // sort: "createDesc" or "createAsc" (matching backend)
    Task<PagedResult<PostDto>> GetPageAsync(int page, int pageSize, string? sort, string? title = null, int? userId = null);

    // more methods...
}