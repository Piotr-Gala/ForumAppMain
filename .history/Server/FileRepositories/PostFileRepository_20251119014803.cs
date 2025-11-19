using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public PostFileRepository() : this("posts.json") { }

    public PostFileRepository(string path)
    {
        _path = string.IsNullOrWhiteSpace(path) ? "posts.json" : path;
        if (!File.Exists(_path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");
            File.WriteAllText(_path, "[]");
        }
    }

    // helpers
    private async Task<List<Post>> LoadAsync()
    {
        if (!File.Exists(_path)) return new List<Post>();
        var json = await File.ReadAllTextAsync(_path);
        var data = JsonSerializer.Deserialize<List<Post>>(json, _json);
        return data ?? new List<Post>();
    }

    private async Task SaveAsync(List<Post> data)
    {
        var json = JsonSerializer.Serialize(data, _json);
        var tmp = _path + ".tmp";
        await File.WriteAllTextAsync(tmp, json);
        if (File.Exists(_path)) File.Delete(_path);
        File.Move(tmp, _path);
    }

    // CRUD operations

    public async Task<Post> AddAsync(Post post)
    {
        if (post is null) throw new ValidationException("Post is null.");
        post.Title = post.Title?.Trim() ?? "";
        post.Body  = post.Body?.Trim()  ?? "";
        if (string.IsNullOrWhiteSpace(post.Title)) throw new ValidationException("Title is required.");
        if (string.IsNullOrWhiteSpace(post.Body))  throw new ValidationException("Body is required.");

        var posts = await LoadAsync();

        post.Id = posts.Count > 0 ? posts.Max(x => x.Id) + 1 : 1;
        posts.Add(post);

        await SaveAsync(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        if (post is null) throw new ValidationException("Post is null.");
        if (post.Id <= 0) throw new ValidationException("Post ID is invalid.");

        var posts = await LoadAsync();

        var idx = posts.FindIndex(p => p.Id == post.Id);
        if (idx < 0) throw new EntityNotFoundException($"Post with ID '{post.Id}' not found.");

        // aktualizacja in-place
        posts[idx].Title = post.Title;
        posts[idx].Body = post.Body;
        posts[idx].UserId = post.UserId;
        posts[idx].LikedByUserIds = post.LikedByUserIds ?? new List<int>();

        await SaveAsync(posts);
    }

    public async Task DeleteAsync(int id)
    {
        var posts = await LoadAsync();

        var idx = posts.FindIndex(p => p.Id == id);
        if (idx < 0) throw new EntityNotFoundException($"Post with ID '{id}' not found.");

        posts.RemoveAt(idx);
        await SaveAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await LoadAsync();
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null) throw new EntityNotFoundException($"Post with ID '{id}' not found.");
        return post;
    }

    public IQueryable<Post> GetManyAsync()
    {
        // File snapshot -> IQueryable on the in-memory list (the only place where we use synchronous code)
        var list = LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        return list.AsQueryable();
    }
}
