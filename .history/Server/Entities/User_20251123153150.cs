namespace Entities;

using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    private User() {} // EF constructor
}
