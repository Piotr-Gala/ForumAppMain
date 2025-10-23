namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ListPostsView
{
    private readonly IPostRepository _posts;
    public ListPostsView(IPostRepository posts) => _posts = posts; //  1 argument (only posts)

    public void Show()
    {
        var list = _posts.GetManyAsync().ToList();
        if (list.Count == 0) { Console.WriteLine("No posts."); return; }

        Console.WriteLine("ID\tTITLE");
        foreach (var p in list) Console.WriteLine($"{p.Id}\t{p.Title}");
    }
}
