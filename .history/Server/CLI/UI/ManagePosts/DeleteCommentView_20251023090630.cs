namespace CLI.UI.ManagePosts;

using CLI.Common;
using RepositoryContracts;

public class DeleteCommentView
{
    private readonly ICommentRepository _comments;

    public DeleteCommentView(ICommentRepository comments)
    {
        _comments = comments;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Delete Comment ===");
        var id = ConsoleUi.ReadInt("Comment ID: ");
        await _comments.DeleteAsync(id);   
        Console.WriteLine("Comment deleted.");
    }
}
