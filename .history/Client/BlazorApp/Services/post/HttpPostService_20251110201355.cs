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
}
