namespace CLI.UI.ManageUsers;

using RepositoryContracts;
using Entities;
using CLI.Common;

public class DeleteUserView
{
    private readonly IUserRepository _users;
    public DeleteUserView(IUserRepository users) => _users = users;

    public async Task ShowAsync()
    {
        Console.WriteLine("=== Delete User ===");
        var id = ConsoleUi.ReadInt("User ID: ");
        
        await _users.DeleteAsync(id);
        Console.WriteLine("User deleted.");
    }
}