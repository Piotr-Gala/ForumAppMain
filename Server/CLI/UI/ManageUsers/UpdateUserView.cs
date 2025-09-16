namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class UpdateUserView
{
    private readonly IUserRepository _users;
    public UpdateUserView(IUserRepository users) => _users = users; //  1 argument (only users)

    public async Task ShowAsync()
    {
        Console.Write("User ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        { Console.WriteLine("Invalid ID."); return; }

        var user = await _users.GetSingleAsync(id);
        if (user == null) { Console.WriteLine("User not found."); return; }

        Console.Write($"New username (current: {user.Username}): ");
        var username = Console.ReadLine();
        Console.Write("New password: ");
        var password = Console.ReadLine();

        // Validate input
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        { Console.WriteLine("Username/password required."); return; }

        user.Username = username;
        user.Password = password;
        await _users.UpdateAsync(user);
        Console.WriteLine("User updated.");
    }
}