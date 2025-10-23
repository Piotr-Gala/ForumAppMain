using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public CommentFileRepository() : this("comments.json") { }

    public CommentFileRepository(string path)
    {
        _path = string.IsNullOrWhiteSpace(path) ? "comments.json" : path;
        if (!File.Exists(_path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");
            File.WriteAllText(_path, "[]");
        }
    }

    // helpers 
    private async Task<List<Comment>> LoadAsync()
    {
        if (!File.Exists(_path)) return new List<Comment>();
        var json = await File.ReadAllTextAsync(_path);
        var data = JsonSerializer.Deserialize<List<Comment>>(json, _json);
        return data ?? new List<Comment>();
    }

    private async Task SaveAsync(List<Comment> data)
    {
        var json = JsonSerializer.Serialize(data, _json);
        var tmp = _path + ".tmp";
        await File.WriteAllTextAsync(tmp, json);
        if (File.Exists(_path)) File.Delete(_path);
        File.Move(tmp, _path);
    }

    // CRUD operations
    public async Task<Comment> AddAsync(Comment comment)
    {
        if (comment is null) throw new ValidationException("Comment is null.");
        comment.Body = comment.Body?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(comment.Body)) throw new ValidationException("Body is required.");

        var comments = await LoadAsync();

        comment.Id = comments.Count > 0 ? comments.Max(x => x.Id) + 1 : 1;
        comments.Add(comment);

        await SaveAsync(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (comment is null) throw new ValidationException("Comment is null.");
        if (comment.Id <= 0) throw new ValidationException("Comment ID is invalid.");

        var comments = await LoadAsync();

        var idx = comments.FindIndex(c => c.Id == comment.Id);
        if (idx < 0) throw new EntityNotFoundException($"Comment with ID '{comment.Id}' not found.");

        comments[idx] = comment;
        await SaveAsync(comments);
    }
    public async Task DeleteAsync(int id)
    {
        var comments = await LoadAsync();

        var idx = comments.FindIndex(c => c.Id == id);
        if (idx < 0) throw new EntityNotFoundException($"Comment with ID '{id}' not found.");

        comments.RemoveAt(idx);
        await SaveAsync(comments);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        var comments = await LoadAsync();
        var comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null) throw new EntityNotFoundException($"Comment with ID '{id}' not found.");
        return comment;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        // File snapshot -> IQueryable on the in-memory list (the only place where we use synchronous code)
        var list = LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        return list.AsQueryable();
    }
}
