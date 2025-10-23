namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;

public class AddCommentView
{
    private readonly ICommentRepository _comments;
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public AddCommentView(ICommentRepository comments, IPostRepository posts, IUserRepository users) // 3 arguments (because comments has postId and userId)
    { _comments = comments; _posts = posts; _users = users; }

    public async Task ShowAsync()
    {
        Console.Write("PostId: ");
        if (!int.TryParse(Console.ReadLine(), out int postId)) { Console.WriteLine("Invalid."); return; }
        Console.Write("UserId: ");
        if (!int.TryParse(Console.ReadLine(), out int userId)) { Console.WriteLine("Invalid."); return; }
        Console.Write("Body: ");
        var body = Console.ReadLine();

        try { _ = await _posts.GetSingleAsync(postId); } catch { Console.WriteLine("Post not found."); return; }
        try { _ = await _users.GetSingleAsync(userId); } catch { Console.WriteLine("User not found."); return; }
        if (string.IsNullOrWhiteSpace(body)) { Console.WriteLine("Body required."); return; }

        var created = await _comments.AddAsync(new Comment { Body = body!, PostId = postId, UserId = userId });
        Console.WriteLine($"Created comment Id={created.Id}");
    }
}
