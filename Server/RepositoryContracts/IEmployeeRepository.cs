using Entities;

namespace RepositoryContracts;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee);
    Task<Employee?> GetByIdAsync(int employeeId);
    Task<List<Employee>> GetAllAsync();
    Task<List<Employee>> GetManyAsync(string? name, int? userId);
    Task<Employee?> GetSingleAsync(int employeeId);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int employeeId);
}