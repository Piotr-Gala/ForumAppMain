namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class UpdatePostView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;
    public UpdatePostView(IPostRepository posts, IUserRepository users) => (_posts, _users) = (posts, users); // 2 arguments (because posts has userId)

    public async Task ShowAsync()
    {
        Console.Write("Post ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        { Console.WriteLine("Invalid ID."); return; }

        var post = await _posts.GetSingleAsync(id);
        //if (post == null) { Console.WriteLine("Post not found."); return; }

        Console.Write("New title (empty to keep): ");
        var title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title)) post.Title = title;

        Console.Write("New body (empty to keep): ");
        var body = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(body)) post.Body = body;

        Console.Write("New User ID (empty to keep): ");
        var userIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(userIdInput))
        {
            if (!int.TryParse(userIdInput, out int userId))
            { Console.WriteLine("Invalid User ID."); return; }

            var user = await _users.GetSingleAsync(userId);
            if (user == null) { Console.WriteLine("User not found."); return; }

            post.UserId = userId;
        }

        await _posts.UpdateAsync(post);
        Console.WriteLine("Post updated.");
    }
}