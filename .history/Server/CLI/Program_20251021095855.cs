using FileRepositories;
using RepositoryContracts;

namespace CLI;

class Program
{
    static async Task Main()
    {
        //(Dependency Injection)
        IUserRepository users = new UserFileRepository();
        IPostRepository posts = new PostFileRepository();
        ICommentRepository comments = new CommentFileRepository();

        var app = new CliApp(users, posts, comments); // ORDER: users, posts, comments
        await app.RunAsync();
    }
}
