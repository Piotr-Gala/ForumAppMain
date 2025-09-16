namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class SinglePostView
{
    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;

    public SinglePostView(IPostRepository posts, ICommentRepository comments) // 2 arguments (because posts has comments)
    { _posts = posts; _comments = comments; }

    public async Task ShowAsync()
    {
        Console.Write("PostId: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid id."); return; }

        try
        {
            var post = await _posts.GetSingleAsync(id);
            Console.WriteLine($"\n[{post.Id}] {post.Title}\n{post.Body}\n");

            var comms = _comments.GetManyAsync().Where(c => c.PostId == post.Id).ToList();
            Console.WriteLine($"Comments ({comms.Count}):");
            foreach (var c in comms) Console.WriteLine($"- ({c.Id}) [u:{c.UserId}] {c.Body}");
        }
        catch { Console.WriteLine("Post not found."); }
    }
}
