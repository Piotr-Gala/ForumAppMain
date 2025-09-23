using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;
        comments.Add(comment);

        string updatedJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, updatedJson);

        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var list = await LoadAsync();
        var idx = list.FindIndex(c => c.Id == comment.Id);
        if (idx < 0) throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        list[idx] = comment;
        await SaveAsync(list);
    }

    public async Task DeleteAsync(int id)
    {
        var list = await LoadAsync();
        var removed = list.RemoveAll(c => c.Id == id);
        if (removed == 0) throw new InvalidOperationException($"Comment with ID '{id}' not found");
        await SaveAsync(list);
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        var list = Load();
        var comment = list.SingleOrDefault(c => c.Id == id)
                      ?? throw new InvalidOperationException($"Comment with ID '{id}' not found");
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync() => Load().AsQueryable();

    private async Task<List<Comment>> LoadAsync()
        => JsonSerializer.Deserialize<List<Comment>>(await File.ReadAllTextAsync(filePath))!;

    private List<Comment> Load()
        => JsonSerializer.Deserialize<List<Comment>>(File.ReadAllTextAsync(filePath).Result)!;  // :contentReference[oaicite:4]{index=4}

    private Task SaveAsync(List<Comment> list)
        => File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
}
