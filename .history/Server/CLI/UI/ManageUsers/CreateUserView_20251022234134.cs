namespace CLI.UI.ManageUsers;

using CLI.Common;          // ConsoleUi
using Entities;
using RepositoryContracts;

public class CreateUserView
{
    private readonly IUserRepository _users;
    public CreateUserView(IUserRepository users) => _users = users;

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Create User ===");

        var username = ConsoleUi.ReadNonEmpty("Username: ").Trim();
        var password = ConsoleUi.ReadNonEmpty("Password: ").Trim();

        var created = await _users.AddAsync(new User
        {
            Username = username,
            Password = password
        });

        Console.WriteLine($"Created user Id={created.Id}");
    }
}
