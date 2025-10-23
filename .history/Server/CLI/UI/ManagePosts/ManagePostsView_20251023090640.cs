namespace CLI.UI.ManagePosts;

using RepositoryContracts;

public class ManagePostsView
{
    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;
    private readonly IUserRepository _users;

    public ManagePostsView(IPostRepository posts, ICommentRepository comments, IUserRepository users)
    { _posts = posts; _comments = comments; _users = users; }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n-- Posts --");
            Console.WriteLine("1) Create post");
            Console.WriteLine("2) List posts");
            Console.WriteLine("3) View single post");
            Console.WriteLine("4) Add comment to post");
            Console.WriteLine("5) Update post");
            Console.WriteLine("6) Delete post");
            Console.WriteLine("7) List posts by userId");
            Console.WriteLine("8) List comments by userId");
            Console.WriteLine("9) Delete comment");   
            Console.WriteLine("B) Back");
            Console.Write("> ");
            var cmd = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (cmd)
            {
                case "1": await new CreatePostView(_posts, _users).ShowAsync(); break;
                case "2": await new ListPostsView(_posts, _users).ShowAsync(); break;
                case "3": await new SinglePostView(_posts, _comments).ShowAsync(); break;
                case "4": await new AddCommentView(_comments, _posts, _users).ShowAsync(); break;
                case "5": await new UpdatePostView(_posts, _users).ShowAsync(); break;
                case "6": await new DeletePostView(_posts).ShowAsync(); break;
                case "7": await new ListPostsByUserIdView(_posts, _users).ShowAsync(); break;   
                case "8": await new ListCommentsByUserIdView(_comments, _users).ShowAsync(); break; 
                case "9": await new DeleteCommentView(_comments).ShowAsync(); break;
                case "b": return;
                default: Console.WriteLine("Unknown."); break;
            }
        }
    }
}
