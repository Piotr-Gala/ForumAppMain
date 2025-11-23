using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Comments;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient client;
    public HttpCommentService(HttpClient client) => this.client = client;

    public async Task<CommentDto> AddAsync(CreateCommentDto request)
    {
        var resp = await client.PostAsJsonAsync("comments", request);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
        return JsonSerializer.Deserialize<CommentDto>(txt, JsonOpts)!;

    }

    public async Task<List<CommentDto>> GetByPostIdAsync(int postId)
        => await client.GetFromJsonAsync<List<CommentDto>>($"comments?postId={postId}") ?? [];

    public async Task<List<CommentDto>> GetByUserIdAsync(int userId)
        => await client.GetFromJsonAsync<List<CommentDto>>($"comments?userId={userId}") ?? [];

    public async Task DeleteAsync(int commentId)
    {
        var resp = await client.DeleteAsync($"comments/{commentId}");
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
    }

    // more methods...
}
