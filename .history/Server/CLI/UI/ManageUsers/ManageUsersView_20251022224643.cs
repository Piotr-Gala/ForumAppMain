namespace CLI.UI.ManageUsers;

using RepositoryContracts;

public class ManageUsersView
{
    private readonly IUserRepository _users;
    public ManageUsersView(IUserRepository users) => _users = users;


    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n-- Users --");
            Console.WriteLine("1) Create user");
            Console.WriteLine("2) List users");
            Console.WriteLine("3) Update user");
            Console.WriteLine("4) Delete user");
            Console.WriteLine("B) Back");
            Console.Write("> ");
            var cmd = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (cmd)
            {
                case "1": await new CreateUserView(_users).ShowAsync(); break;
                case "2": await new ListUsersView(_users).ShowAsync(); break;
                case "3": await new UpdateUserView(_users).ShowAsync(); break;   
                case "4": await new DeleteUserView(_users).ShowAsync(); break;
                case "b": return;
                default: Console.WriteLine("Unknown."); break;
            }
        }
    }
}
