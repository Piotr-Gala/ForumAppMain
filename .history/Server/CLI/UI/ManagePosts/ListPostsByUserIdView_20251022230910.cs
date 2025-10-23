namespace CLI.UI.ManagePosts;

using RepositoryContracts;
using CLI.Common;

public class ListPostsByUserIdView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public ListPostsByUserIdView(IPostRepository posts, IUserRepository users)
    { _posts = posts; _users = users; }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Posts by User ===");
        var userId = ConsoleUi.ReadInt("UserId: ");

        var user = await _users.GetSingleAsync(userId);

        Console.WriteLine($"\nUser {user.Id} ({user.Username}) — posts:");
        foreach (var p in _posts.GetManyAsync().Where(p => p.UserId == userId).OrderBy(p => p.Id))
            Console.WriteLine($"{p.Id,3}  {p.Title}");
    }
}
