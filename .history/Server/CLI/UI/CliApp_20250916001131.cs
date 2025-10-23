using RepositoryContracts;
using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts;

namespace CLI;

public class CliApp
{
    private readonly IUserRepository _users;
    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;

    public CliApp(IUserRepository users, IPostRepository posts, ICommentRepository comments)
    {
        _users = users;
        _posts = posts;
        _comments = comments;
    }

    public async Task RunAsync()
    {
        var usersView = new ManageUsersView(_users);
        var postsView = new ManagePostsView(_posts, _comments, _users);

        while (true)
        {
            Console.WriteLine("\n=== Forum CLI ===");
            Console.WriteLine("1) Manage users");
            Console.WriteLine("2) Manage posts");
            Console.WriteLine("X) Exit");
            Console.Write("> ");
            var cmd = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (cmd)
            {
                case "1": await usersView.ShowAsync(); break;
                case "2": await postsView.ShowAsync(); break;
                case "x": return;
                default: Console.WriteLine("Unknown command."); break;
            }
        }
    }
}
