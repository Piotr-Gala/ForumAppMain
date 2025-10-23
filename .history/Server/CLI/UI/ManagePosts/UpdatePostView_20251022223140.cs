namespace CLI.UI.ManagePosts;

using RepositoryContracts;
using Entities;
using CLI.Common;

public class UpdatePostView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;
    public UpdatePostView(IPostRepository posts, IUserRepository users) 
    {
        _posts = posts;
        _users = users;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Update Post ===");

        var id = ConsoleUi.ReadInt("Post ID: ");
        var post = await _posts.GetSingleAsync(id);

        Console.WriteLine("Leave empty to keep current value.");
        var newUserIdStr = ConsoleUi.ReadOptional($"Author UserId (current: {post.UserId}): ");
        var newTitle     = ConsoleUi.ReadOptional($"Title (current: {post.Title}): ");
        var newBody      = ConsoleUi.ReadOptional("Body (current shown in edit is hidden): ");

        if (newUserIdStr is not null)
        {
            if (!int.TryParse(newUserIdStr, out var uid))
                throw new ValidationException("Author UserId must be a number.");
            // weryfikacja istnienia autora
            _ = await _users.GetSingleAsync(uid);
            post.UserId = uid;
        }

        if (newTitle is not null) post.Title = newTitle.Trim();
        if (newBody  is not null) post.Body  = newBody.Trim();

        await _posts.UpdateAsync(post);
        Console.WriteLine("Post updated.");
    }
}