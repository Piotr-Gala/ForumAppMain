namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;
using CLI.Common;

public class AddCommentView
{
    private readonly ICommentRepository _comments;
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public AddCommentView(ICommentRepository comments, IPostRepository posts, IUserRepository users) // 3 arguments (because comments has postId and userId)
    { _comments = comments; _posts = posts; _users = users; }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Add Comment ===");

        var postId = ConsoleUi.ReadInt("Post ID: ");
        var userId = ConsoleUi.ReadInt("Author UserId: ");
        var body   = ConsoleUi.ReadNonEmpty("Body: ");

        // weryfikacje powiązań (jeśli brak -> EntityNotFoundException)
        _ = await _posts.GetSingleAsync(postId);
        _ = await _users.GetSingleAsync(userId);


        var created = await _comments.AddAsync(new Comment
        {
            PostId = postId,
            UserId = userId,
            Body = body.Trim()
        });
        
        Console.WriteLine($"Created comment Id={created.Id}");
    }
}
