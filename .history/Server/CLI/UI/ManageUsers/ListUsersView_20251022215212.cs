namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class ListUsersView
{
    private readonly IUserRepository _users;
    public ListUsersView(IUserRepository users) => _users = users; 

    public Task Show()
    {
        Console.WriteLine("Users");
        foreach (var u in _users.GetManyAsync().OrderBy(u => u.Id))
            Console.WriteLine($"{u.Id}\t{u.Username}");
        return Task.CompletedTask;
    }
}
