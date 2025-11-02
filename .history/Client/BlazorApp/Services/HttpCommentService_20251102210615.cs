using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts.Comments;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;
    public HttpCommentService(HttpClient client) => this.client = client;

    public async Task<CommentDto> AddAsync(CreateCommentDto request)
    {
        var resp = await client.PostAsJsonAsync("comments", request);
        var txt  = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);

        return JsonSerializer.Deserialize<CommentDto>(txt, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetByPostIdAsync(int postId)
        => await client.GetFromJsonAsync<List<CommentDto>>($"comments?postId={postId}") ?? [];
}
