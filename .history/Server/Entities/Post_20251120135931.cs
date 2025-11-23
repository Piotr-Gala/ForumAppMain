namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public int UserId { get; set; } // author
    public List<int> LikedByUserIds { get; set; } = new();

    private Post() {}
}
