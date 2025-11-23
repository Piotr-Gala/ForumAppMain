using Entities;

namespace RepositoryContracts;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee);
    Task<Employee?> GetByIdAsync(int employeeId);
    Task<List<Employee>> GetAllAsync();
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int employeeId);
}