using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public UserFileRepository() : this("users.json") { }

    public UserFileRepository(string path)
    {
        _path = string.IsNullOrWhiteSpace(path) ? "users.json" : path;
        if (!File.Exists(_path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");
            File.WriteAllText(_path, "[]");
        }
    }

// helpers
    private async Task<List<User>> LoadAsync()
    {
        if (!File.Exists(_path)) return new List<User>();
        var json = await File.ReadAllTextAsync(_path);
        var data = JsonSerializer.Deserialize<List<User>>(json, _json);
        return data ?? new List<User>();
    }

    private async Task SaveAsync(List<User> data)
    {
        var json = JsonSerializer.Serialize(data, _json);
        var tmp = _path + ".tmp";
        await File.WriteAllTextAsync(tmp, json);
        if (File.Exists(_path)) File.Delete(_path);
        File.Move(tmp, _path);
    }
    // CRUD operations
    public async Task<User> AddAsync(User user)
    {
        if (user is null) throw new ValidationException("User is null.");
        if (string.IsNullOrWhiteSpace(user.Username)) throw new ValidationException("Username is required.");

        var users = await LoadAsync();

        if (users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateEntityException($"User '{user.Username}' already exists.");

        user.Id = users.Count > 0 ? users.Max(x => x.Id) + 1 : 1;
        users.Add(user);

        await SaveAsync(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        if (user is null) throw new ValidationException("User is null.");
        if (user.Id <= 0) throw new ValidationException("User ID is invalid.");
        if (string.IsNullOrWhiteSpace(user.Username)) throw new ValidationException("Username is required.");

        var users = await LoadAsync();

        var idx = users.FindIndex(u => u.Id == user.Id);
        if (idx < 0) throw new EntityNotFoundException($"User with ID '{user.Id}' not found.");

        if (users.Any(u => u.Id != user.Id &&
                           u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateEntityException($"User '{user.Username}' already exists.");

        // aktualizacja in-place
        users[idx].Username = user.Username;
        users[idx].Password = user.Password;

        await SaveAsync(users);
    }

    public async Task DeleteAsync(int id)
    {
        var users = await LoadAsync();

        var idx = users.FindIndex(u => u.Id == id);
        if (idx < 0) throw new EntityNotFoundException($"User with ID '{id}' not found.");

        users.RemoveAt(idx);
        await SaveAsync(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var users = await LoadAsync();
        var user = users.SingleOrDefault(u => u.Id == id);
        if (user is null) throw new EntityNotFoundException($"User with ID '{id}' not found.");
        return user;
    }

    public IQueryable<User> GetManyAsync()
    {
        // File snapshot -> IQueryable on the in-memory list (the only place where we use synchronous code)
        var list = LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        return list.AsQueryable();
    }
}
