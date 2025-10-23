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
        var usersDict = _users.GetManyAsync().ToDictionary(u => u.Id, u => u.Username);
        foreach (var p in _posts.GetManyAsync().OrderBy(p => p.Id))
        {
            usersDict.TryGetValue(p.UserId, out var username);
            Console.WriteLine($"{p.Id} \t[{p.UserId}:{username ?? "?"}] \t{p.Title}");
        }
        return Task.CompletedTask;
    }
}
