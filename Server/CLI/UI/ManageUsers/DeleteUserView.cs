namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class DeleteUserView
{
    private readonly IUserRepository _users;
    public DeleteUserView(IUserRepository users) => _users = users;

    public async Task ShowAsync()
    {
        Console.Write("User ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        { Console.WriteLine("Invalid ID."); return; }

        var user = await _users.GetSingleAsync(id);
        if (user == null) { Console.WriteLine("User not found."); return; }

        await _users.DeleteAsync(id);
        Console.WriteLine("User deleted.");
    }
}