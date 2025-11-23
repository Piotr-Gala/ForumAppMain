namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContracts;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employee;

    public EmployeeController(IEmployeeRepository employee) => _employee = employee;

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto request)
    {
       
        var created = await _employee.AddAsync(new Employee { Name = request.Name, Position = request.Position, Salary = request.Salary });

        var dto = new EmployeeDto { Id = created.Id, Name = created.Name, Position = created.Position, Salary = created.Salary };
        return Created($"/Employees/{dto.Id}", dto);
    }

    // GET 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetSingle(int id)
    {
        var p = await _employee.GetSingleAsync(id);
        if (p is null) return NotFound();

        return Ok(new EmployeeDto { Id = p.Id, Name = p.Name, Position = p.Position, Salary = p.Salary });
    }

    // GET 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetMany(
        [FromQuery] string? name,
        [FromQuery] int? userId)
    {
        IEnumerable<Employee> q = await _employee.GetManyAsync(name, userId); 

        if (!string.IsNullOrWhiteSpace(name))  
            q = q.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        if (userId is not null) 
            q = q.Where(p => p.Id == userId.Value);

        var result = q.Select(p => new EmployeeDto { Id = p.Id, Name = p.Name, Position = p.Position, Salary = p.Salary })
                      .ToList();

        return Ok(result);
    }

    // PUT 
    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] UpdateEmployeeDto request)
    {
        var p = await _employee.GetSingleAsync(id);
        if (p is null) return NotFound();

        p.Name = request.Name ?? p.Name;
        p.Position = request.Position ?? p.Position;
        p.Salary = request.Salary ?? p.Salary;

        var updated =  await _employee.UpdateAsync(p);

        var dto = new EmployeeDto { Id = updated.Id, Name = updated.Name, Position = updated.Position, Salary = updated.Salary };
        return Ok(dto);
    }

    // DELETE 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employee.DeleteAsync(id);
        return NoContent();
    }
    



    // CRUD endpoints would go here
}