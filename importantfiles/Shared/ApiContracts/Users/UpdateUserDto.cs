namespace ApiContracts.Users;

public class UpdateUserDto
{
    public required string Username { get; set; }
    public string? Password { get; set; } // null = don't change
}
