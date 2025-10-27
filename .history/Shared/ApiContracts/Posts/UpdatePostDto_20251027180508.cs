namespace ApiContracts.Posts;

public class UpdatePostDto
{
    public required string Title { get; set; }
    public string? Content { get; set; } // null = don't change
    public string? Author { get; set; } // null = don't change
}
