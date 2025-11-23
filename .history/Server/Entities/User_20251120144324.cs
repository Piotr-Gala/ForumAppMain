namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    private User() {} // EF constructor

    public ICollection<Post> Posts { get; set; }  // user’s posts
        = new List<Post>();                       // init collection

    public ICollection<Comment> Comments { get; set; } // user’s comments
        = new List<Comment>();                         // init collection
}
