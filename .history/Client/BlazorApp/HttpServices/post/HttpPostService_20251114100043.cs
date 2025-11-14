using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Posts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;
    public HttpPostService(HttpClient client) => this.client = client;

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var resp = await client.PostAsJsonAsync("posts", request);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);

        return JsonSerializer.Deserialize<PostDto>(txt, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<PostDto>> GetAllAsync()
        => await client.GetFromJsonAsync<List<PostDto>>("posts") ?? [];

    public async Task<PostDto> GetByIdAsync(int id)
        => await client.GetFromJsonAsync<PostDto>($"posts/{id}")
           ?? throw new Exception("Post not found");


    // NEW: update
    public async Task UpdateAsync(int id, UpdatePostDto request)
    {
        var resp = await client.PutAsJsonAsync($"posts/{id}", request);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
    }

    // Paging + Sorting
    public async Task<PagedResultDto<PostDto>> GetPageAsync(int page, int pageSize, string? sort, string? title = null, int? userId = null)
    {
        // Easy, readable URL; without any "glueing" of list
        var url = $"posts/paged?page={page}&pageSize={pageSize}&sort={sort}";
        if (!string.IsNullOrWhiteSpace(title)) url += $"&title={Uri.EscapeDataString(title)}";
        if (userId is not null) url += $"&userId={userId}";
        var resp = await client.GetAsync(url);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);

        return JsonSerializer.Deserialize<PagedResultDto<PostDto>>(txt, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    // NEW: /Posts?title=...
    public async Task<List<PostDto>> GetByTitleAsync(string title)
        => await client.GetFromJsonAsync<List<PostDto>>($"posts?title={title}") ?? [];

    // NEW: /Posts?userId=123
    public async Task<List<PostDto>> GetByUserIdAsync(int userId)
        => await client.GetFromJsonAsync<List<PostDto>>($"posts?userId={userId}") ?? [];

    // NEW: delete
    public async Task DeleteAsync(int id)
    {
        var resp = await client.DeleteAsync($"posts/{id}");
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
    }

    // more methods...
}
