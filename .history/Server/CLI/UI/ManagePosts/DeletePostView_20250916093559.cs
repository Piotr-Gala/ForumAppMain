namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class DeletePostView
{
    private readonly IPostRepository _posts;
    public DeletePostView(IPostRepository posts) => _posts = posts;

    public async Task ShowAsync()
    {
        Console.Write("PostId to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid id."); return; }

        try { await _posts.DeleteAsync(id); Console.WriteLine("Post deleted."); }
        catch { Console.WriteLine("Post not found."); }
    }
}
