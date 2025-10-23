namespace CLI.UI.ManagePosts;

using CLI.Common;          
using Entities;
using RepositoryContracts;

public class CreatePostView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public CreatePostView(IPostRepository posts, IUserRepository users)
    { _posts = posts; _users = users; }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Create Post ===");

        var userId = ConsoleUi.ReadInt("Author UserId: ");
        var title  = ConsoleUi.ReadNonEmpty("Title: ");
        var body   = ConsoleUi.ReadNonEmpty("Body: ");

        _ = await _users.GetSingleAsync(userId);

        var created = await _posts.AddAsync(new Post
        {
            UserId = userId,
            Title  = title.Trim(),
            Body   = body.Trim()
        });

        Console.WriteLine($"Created post Id={created.Id}");
    }
}
