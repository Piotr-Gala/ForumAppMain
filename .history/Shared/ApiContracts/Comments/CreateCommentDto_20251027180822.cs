namespace ApiContracts.Comments;

public class CreateCommentDto
{
    public required string Body { get; set; }
    public required string Author { get; set; }
}
