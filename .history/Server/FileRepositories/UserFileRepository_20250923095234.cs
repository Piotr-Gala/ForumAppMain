using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    public async Task<User> AddAsync(User user)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;

        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);

        string updated = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, updated);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;

        int idx = users.FindIndex(u => u.Id == user.Id);
        if (idx < 0) throw new InvalidOperationException($"User with ID '{user.Id}' not found");

        users[idx] = user;

        string updated = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, updated);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;

        int removed = users.RemoveAll(u => u.Id == id);
        if (removed == 0) throw new InvalidOperationException($"User with ID '{id}' not found");

        string updated = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, updated);
    }

    public Task<User> GetSingleAsync(int id)
    {
        string json = File.ReadAllText(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;

        var user = users.SingleOrDefault(u => u.Id == id)
                   ?? throw new InvalidOperationException($"User with ID '{id}' not found");
        return Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        string json = File.ReadAllText(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;
        return users.AsQueryable();
    }
}
