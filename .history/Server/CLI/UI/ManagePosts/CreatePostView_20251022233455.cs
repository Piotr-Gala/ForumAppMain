namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;

public class CreatePostView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;

    public CreatePostView(IPostRepository posts, IUserRepository users)  //  2 arguments (because posts has userId)
    { _posts = posts; _users = users; }

    public async Task ShowAsync()
    {
        Console.Write("Author UserId: ");
        if (!int.TryParse(Console.ReadLine(), out int userId)) { Console.WriteLine("Invalid id."); return; }
        try { _ = await _users.GetSingleAsync(userId); } catch { Console.WriteLine("User not found."); return; }

        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Body: ");
        var body = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body)) { Console.WriteLine("Title/body required."); return; }

        var created = await _posts.AddAsync(new Post { Title = title!, Body = body!, UserId = userId });
        Console.WriteLine($"Created post Id={created.Id}");
    }
}
