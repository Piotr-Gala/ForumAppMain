namespace CLI.UI.ManagePosts;

using RepositoryContracts;
using Entities;
using CLI.Common;

public class DeletePostView
{
    private readonly IPostRepository _posts;
    public DeletePostView(IPostRepository posts) => _posts = posts;

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Delete Post ===");
        var id = ConsoleUi.ReadInt("Post ID: ");
        await _posts.DeleteAsync(id);
        Console.WriteLine("Post deleted.");
    }
}
