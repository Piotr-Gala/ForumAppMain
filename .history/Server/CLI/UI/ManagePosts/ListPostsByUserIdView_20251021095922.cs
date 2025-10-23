namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ListPostsByUserIdView
{
    private readonly IPostRepository _posts;
    public ListPostsByUserIdView(IPostRepository posts) => _posts = posts;

    public void Show()
    {
        Console.Write("UserId: ");
        if (!int.TryParse(Console.ReadLine(), out int userId)) { Console.WriteLine("Invalid."); return; }

        var list = _posts.GetManyAsync().Where(p => p.UserId == userId).ToList();
        if (list.Count == 0) { Console.WriteLine("No posts for this user."); return; }

        Console.WriteLine("ID\tTITLE");
        foreach (var p in list) Console.WriteLine($"{p.Id}\t{p.Title}");
    }
}
