namespace CLI.UI.ManagePosts;

using Entities;
using RepositoryContracts;

public class UpdatePostView
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;
    public UpdatePostView(IPostRepository posts, IUserRepository users) => (_posts, _users) = (posts, users); // 2 arguments (because posts has userId)

    public async Task ShowAsync()
    {
        Console.Write("Post ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        { 
            Console.WriteLine("Invalid ID."); 
            return; 
        }

        // --- CHANGED: repo rzuca gdy brak wpisu, więc łapiemy wyjątek zamiast sprawdzać null
        Post post;
        try
        {
            post = await _posts.GetSingleAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message); // np. "Post with ID '10' not found"
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while loading post: {ex.Message}");
            return;
        }

        Console.Write("New title (empty to keep): ");
        var title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title)) post.Title = title;

        Console.Write("New body (empty to keep): ");
        var body = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(body)) post.Body = body;

        Console.Write("New User ID (empty to keep): ");
        var userIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(userIdInput))
        {
            if (!int.TryParse(userIdInput, out var userId))
            { 
                Console.WriteLine("Invalid User ID."); 
                return; 
            }

            // --- CHANGED: to też rzuca gdy user nie istnieje
            try
            {
                _ = await _users.GetSingleAsync(userId);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("User not found.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading user: {ex.Message}");
                return;
            }

            post.UserId = userId;
        }

        // --- CHANGED: bezpieczne zapisanie
        try
        {
            await _posts.UpdateAsync(post);
            Console.WriteLine("Post updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while updating: {ex.Message}");
        }
    }
}
