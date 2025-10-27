namespace ApiContracts.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public required string Author { get; set; }
}
