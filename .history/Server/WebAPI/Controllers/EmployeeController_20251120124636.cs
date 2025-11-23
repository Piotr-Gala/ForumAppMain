namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetAll()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return Ok(employees);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create([FromBody] Employee employee)
    {
        var createdEmployee = await _employeeRepository.AddAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Employee>> Update(int id, [FromBody] Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest("Employee ID mismatch.");
        }

        var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
        return Ok(updatedEmployee);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _employeeRepository.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    
    // CRUD endpoints would go here
}