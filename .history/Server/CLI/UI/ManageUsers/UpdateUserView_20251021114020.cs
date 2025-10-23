namespace CLI.UI.ManageUsers;

using RepositoryContracts;
using Entities;

public class UpdateUserView
{
    private readonly IUserRepository _users;
    public UpdateUserView(IUserRepository users) => _users = users; //  1 argument (only users)

    public async Task ShowAsync()
    {
        Console.Write("User ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
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

        Console.WriteLine("empty to keep");
        Console.Write($"New username (current: {user.Username}): ");
        var username = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(username)) user.Username = username;

        Console.Write("New password: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password)) password = user.Password;

        await _users.UpdateAsync(user);
        Console.WriteLine("User updated.");
        }
}