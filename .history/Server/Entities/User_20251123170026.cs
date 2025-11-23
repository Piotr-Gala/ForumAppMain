namespace Entities;

using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    //navigation properties
    public ICollection<Post> Posts { get; set; } // user posts
        = new List<Post>();                       // init collection

    public ICollection<Comment> Comments { get; set; } // user comments
        = new List<Comment>();                       // init collection

    public User() {} // for CLI / in-memory / EF
}
