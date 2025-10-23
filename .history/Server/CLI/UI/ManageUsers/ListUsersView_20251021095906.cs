namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class ListUsersView
{
    private readonly IUserRepository _users;
    public ListUsersView(IUserRepository users) => _users = users; 

    public void Show()
    {
        var list = _users.GetManyAsync().ToList();
        if (list.Count == 0) { Console.WriteLine("No users."); return; }

        Console.WriteLine("ID\tUSERNAME");
        foreach (var u in list) Console.WriteLine($"{u.Id}\t{u.Username}");
    }
}
