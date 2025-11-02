using ApiContracts.Comments;
using System.Text.Json;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;
    public HttpCommentService(HttpClient client) => this.client = client;

    public async Task<CommentDto> AddAsync(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetByPostIdAsync(int postId)
        => await client.GetFromJsonAsync<List<CommentDto>>($"comments?postId={postId}") ?? [];
}