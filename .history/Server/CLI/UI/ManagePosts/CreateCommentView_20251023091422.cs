namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;
using CLI.Common;

public class CreateCommentView
{
    private readonly ICommentRepository _comments;
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public CreateCommentView(ICommentRepository comments, IPostRepository posts, IUserRepository users) // 3 arguments (because comments has postId and userId)
    { _comments = comments; _posts = posts; _users = users; }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Create Comment ===");

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
