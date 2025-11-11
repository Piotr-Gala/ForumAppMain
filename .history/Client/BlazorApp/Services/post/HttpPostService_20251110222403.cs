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

    // NEW: pobieranie z filtrami zgodnie z API (title/userId)
    public async Task<List<PostDto>> GetFilteredAsync(string? title = null, int? userId = null)
    {
        var qs = new List<string>();
        if (!string.IsNullOrWhiteSpace(title)) qs.Add($"title={Uri.EscapeDataString(title)}");
        if (userId is not null) qs.Add($"userId={userId}");
        var url = "posts" + (qs.Count > 0 ? "?" + string.Join("&", qs) : "");
        return await client.GetFromJsonAsync<List<PostDto>>(url) ?? [];
    }

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
