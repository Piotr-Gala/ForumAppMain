namespace CLI.UI.ManagePosts;

using RepositoryContracts;
using CLI.Common;

public class SinglePostView
{
    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;

    public SinglePostView(IPostRepository posts, ICommentRepository comments)
    { _posts = posts; _comments = comments; }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== View Post ===");
        var id = ConsoleUi.ReadInt("Post ID: ");

        var post = await _posts.GetSingleAsync(id); // EntityNotFoundException -> złapie centralny handler

        Console.WriteLine($"\n[{post.Id}] (user {post.UserId}) {post.Title}");
        Console.WriteLine(post.Body);

        Console.WriteLine("\n-- Comments --");
        var comments = _comments.GetManyAsync()
                                .Where(c => c.PostId == post.Id)
                                .OrderBy(c => c.Id);
        foreach (var c in comments)
            Console.WriteLine($"{c.Id,3}  (user {c.UserId})  {c.Body}");

    }
}
