namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ListPostsView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public ListPostsView(IPostRepository posts, IUserRepository users)
    {
        _posts = posts;
        _users = users;
    }

    public Task ShowAsync()
    {
        Console.WriteLine("=== Posts List ===");
        var usersById = _users.GetManyAsync().ToDictionary(u => u.Id, u => u.Username);
        foreach (var p in _posts.GetManyAsync().OrderBy(p => p.Id))
        {
            usersById.TryGetValue(p.UserId, out var uname);
            Console.WriteLine($"{p.Id,3}  [{p.UserId}:{uname ?? "?"}]  {p.Title}");
        }
        return Task.CompletedTask;
    }
}
