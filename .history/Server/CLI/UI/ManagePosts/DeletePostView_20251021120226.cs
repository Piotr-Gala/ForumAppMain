namespace CLI.UI.ManagePosts;

using RepositoryContracts;
using Entities;

public class DeletePostView
{
    private readonly IPostRepository _posts;
    public DeletePostView(IPostRepository posts) => _posts = posts;

    public async Task ShowAsync()
    {
        Console.Write("PostId to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid id."); return; }

        Post post;
        try
        {
            post = await _posts.GetSingleAsync(id); // <-- tu wcześniej wybuchało
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message); // "Post with ID '5' not found"
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while loading post: {ex.Message}");
            return;
        }

        try { await _posts.DeleteAsync(id); Console.WriteLine("Post deleted."); }
        catch { Console.WriteLine("Post not found."); }
    }
}
