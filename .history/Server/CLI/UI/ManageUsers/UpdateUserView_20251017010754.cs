namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class UpdateUserView
{
    private readonly IUserRepository _users;
    public UpdateUserView(IUserRepository users) => _users = users;

    public async Task ShowAsync()
    {
        Console.Write("User ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        { 
            Console.WriteLine("Invalid ID."); 
            return; 
        }

        var user = await _users.GetSingleAsync(id);
        if (user == null)
        { 
            Console.WriteLine("User not found."); 
            return; 
        }

        Console.WriteLine("empty to keep");
        Console.Write($"New username (current: {user.Username}): ");
        var username = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(username))
            user.Username = username;

        Console.Write("New password: ");
        var password = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(password))
            user.Password = password;

        try
        {
            await _users.UpdateAsync(user);
            Console.WriteLine("User updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while updating: {ex.Message}");
        }
    }
}
