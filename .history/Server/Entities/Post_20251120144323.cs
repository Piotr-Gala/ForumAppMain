namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public int UserId { get; set; } // author
    public List<int> LikedByUserIds { get; set; } = new();

    private Post() {}

    public User User { get; set; } = null!;    // navigation User

    public ICollection<Comment> Comments { get; set; } // post comments
        = new List<Comment>();                         // init collection
}
