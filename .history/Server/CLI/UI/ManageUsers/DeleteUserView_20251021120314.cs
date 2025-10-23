namespace CLI.UI.ManageUsers;

using RepositoryContracts;
using Entities;

public class DeleteUserView
{
    private readonly IUserRepository _users;
    public DeleteUserView(IUserRepository users) => _users = users;

    public async Task ShowAsync()
    {
        Console.Write("User ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        { Console.WriteLine("Invalid ID."); return; }

        User user;
        try
        {
            user = await _users.GetSingleAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message); // "User with ID '5' not found"
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while loading user: {ex.Message}");
            return;
        }

        await _users.DeleteAsync(id);
        Console.WriteLine("User deleted.");
    }
}