namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ListCommentsByUserIdView
{
    private readonly ICommentRepository _comments;
    public ListCommentsByUserIdView(ICommentRepository comments) => _comments = comments;

    public void Show()
    {
        Console.Write("UserId: ");
        if (!int.TryParse(Console.ReadLine(), out int userId)) { Console.WriteLine("Invalid."); return; }

        var list = _comments.GetManyAsync().Where(c => c.UserId == userId).ToList();
        if (list.Count == 0) { Console.WriteLine("No comments by this user."); return; }

        Console.WriteLine("ID\tPOST\tBODY");
        foreach (var c in list) Console.WriteLine($"{c.Id}\t{c.PostId}\t{c.Body}");
    }
}
