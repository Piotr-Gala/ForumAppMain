using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    public async Task<Post> AddAsync(Post post)
    {
        var list = await LoadAsync();
        var nextId = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        post.Id = nextId;
        list.Add(post);
        await SaveAsync(list);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var list = await LoadAsync();
        var idx = list.FindIndex(p => p.Id == post.Id);
        if (idx < 0) throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        list[idx] = post;
        await SaveAsync(list);
    }

    public async Task DeleteAsync(int id)
    {
        var list = await LoadAsync();
        var removed = list.RemoveAll(p => p.Id == id);
        if (removed == 0) throw new InvalidOperationException($"Post with ID '{id}' not found");
        await SaveAsync(list);
    }

    public Task<Post> GetSingleAsync(int id)
    {
        var list = Load();
        var post = list.SingleOrDefault(p => p.Id == id)
                   ?? throw new InvalidOperationException($"Post with ID '{id}' not found");
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync() => Load().AsQueryable();

    private async Task<List<Post>> LoadAsync()
    {
        var json = await File.ReadAllTextAsync(filePath);
        if (string.IsNullOrWhiteSpace(json)) return new List<Post>();
        try
        {
            return JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
        }
        catch (JsonException)
        {
            return new List<Post>();
        }
    }

    private List<Post> Load()
    {
        var json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json)) return new List<Post>();
        try
        {
            return JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
        }
        catch (JsonException)
        {
            return new List<Post>();
        }
    }

    private Task SaveAsync(List<Post> list)
        => File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
}
