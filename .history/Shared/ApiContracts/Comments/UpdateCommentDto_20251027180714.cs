namespace ApiContracts.Comments;

public class UpdateCommentDto
{
    public required string Content { get; set; }
    public string? Author { get; set; } // null = don't change
}
