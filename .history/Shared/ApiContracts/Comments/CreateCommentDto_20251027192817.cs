namespace ApiContracts.Comments;

public class CreateCommentDto
{
    public required string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}
