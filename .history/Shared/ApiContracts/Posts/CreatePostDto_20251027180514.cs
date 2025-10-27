namespace ApiContracts.Posts;

public class CreatePostDto
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
}
