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

    

    // CRUD endpoints would go here
}