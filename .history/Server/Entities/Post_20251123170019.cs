namespace Entities;

using System.Collections.Generic;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public int UserId { get; set; } // author
    public List<int> LikedByUserIds { get; set; } = new();

    //navigation properties
    public User User { get; private set; } = null!; // post author

    public ICollection<Comment> Comments { get; private set; } // post comments
        = new List<Comment>();                       // init collection

    public Post() {} // for CLI / in-memory / EF
}
