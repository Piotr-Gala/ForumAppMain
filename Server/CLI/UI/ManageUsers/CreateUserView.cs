namespace CLI.UI.ManageUsers;

using Entities;
using RepositoryContracts;

public class CreateUserView
{
    private readonly IUserRepository _users;
    public CreateUserView(IUserRepository users) => _users = users; 

    public async Task ShowAsync()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

            // Validate input
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        { Console.WriteLine("Username/password required."); return; }

            // Prevent duplicate usernames
        if (_users.GetManyAsync().Any(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Username already taken.");
            return;
        }


        var created = await _users.AddAsync(new User { Username = username!, Password = password! });
        Console.WriteLine($"Created user Id={created.Id}");
    }
}
