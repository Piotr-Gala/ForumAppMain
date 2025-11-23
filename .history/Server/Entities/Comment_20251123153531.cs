namespace Entities;

using System.Collections.Generic;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; } = null!;
    public int UserId { get; set; }   // author
    public int PostId { get; set; }   // parent post

    //navigation properties
    public User User { get; private set; } = null!; // comment author
    public Post Post { get; private set; } = null!; // parent post

    private Comment() {} // EF constructor
}

