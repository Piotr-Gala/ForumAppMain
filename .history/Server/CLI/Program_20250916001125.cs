using InMemoryRepositories;
using RepositoryContracts;

namespace CLI;

class Program
{
    static async Task Main()
    {
        //(Dependency Injection)
        IUserRepository users = new UserInMemoryRepository();
        IPostRepository posts = new PostInMemoryRepository();
        ICommentRepository comments = new CommentInMemoryRepository();

        var app = new CliApp(users, posts, comments); // ORDER: users, posts, comments
        await app.RunAsync();
    }
}
