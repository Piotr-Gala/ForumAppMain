namespace Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public decimal Salary { get; set; }
}