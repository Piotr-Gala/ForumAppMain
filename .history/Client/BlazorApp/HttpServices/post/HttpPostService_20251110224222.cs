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

    // NEW: /Posts?title=...
    public async Task<List<PostDto>> GetByTitleAsync(string title)
        => await client.GetFromJsonAsync<List<PostDto>>($"posts?title={title}") ?? [];

    // NEW: /Posts?userId=123
    public async Task<List<PostDto>> GetByUserIdAsync(int userId)
        => await client.GetFromJsonAsync<List<PostDto>>($"posts?userId={userId}") ?? [];

    // NEW: update
    public async Task UpdateAsync(int id, UpdatePostDto request)
    {
        var resp = await client.PutAsJsonAsync($"posts/{id}", request);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
    }

    // NEW: delete
    public async Task DeleteAsync(int id)
    {
        var resp = await client.DeleteAsync($"posts/{id}");
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
    }

    // more methods...
}
