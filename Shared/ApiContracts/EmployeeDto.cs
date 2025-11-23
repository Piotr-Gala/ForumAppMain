namespace ApiContracts;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public decimal Salary { get; set; }
}

public class CreateEmployeeDto
{
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public decimal Salary { get; set; }
}

public class UpdateEmployeeDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Position { get; set; }
    public decimal? Salary { get; set; }
}

