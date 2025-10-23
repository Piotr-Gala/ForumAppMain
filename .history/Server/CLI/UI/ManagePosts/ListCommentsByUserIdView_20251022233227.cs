namespace CLI.UI.ManagePosts;

using RepositoryContracts;
using CLI.Common;

public class ListCommentsByUserIdView
{
    private readonly ICommentRepository _comments;
    private readonly IUserRepository _users;
    public ListCommentsByUserIdView(ICommentRepository comments, IUserRepository users) { _comments = comments; _users = users; }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Comments by User ===");
        var userId = ConsoleUi.ReadInt("UserId: "); 

        var user = await _users.GetSingleAsync(userId);

        Console.WriteLine($"\nUser {user.Id} ({user.Username}) — comments:");
        foreach (var c in _comments.GetManyAsync().Where(c => c.UserId == userId).OrderBy(c => c.Id))
            Console.WriteLine($"{c.Id,3}  Post:{c.PostId,3}  {ConsoleUi.Truncate(c.Body, 60)}");

    }
}
