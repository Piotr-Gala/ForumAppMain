using RepositoryContracts;
using Entities;
using System.Text.Json;

namespace FileRepositories;

public class EmployeeFileRepository : IEmployeeRepository
{
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public EmployeeFileRepository() : this("employees.json") { }

    public EmployeeFileRepository(string path)
    {
        _path = string.IsNullOrWhiteSpace(path) ? "employees.json" : path;
        if (!File.Exists(_path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");
            File.WriteAllText(_path, "[]");
        }
    }

    // helpers
    private async Task<List<Employee>> LoadAsync()
    {
        if (!File.Exists(_path)) return new List<Employee>();
        var json = await File.ReadAllTextAsync(_path);
        var data = JsonSerializer.Deserialize<List<Employee>>(json, _json);
        return data ?? new List<Employee>();
    }

    private async Task SaveAsync(List<Employee> data)
    {
        var json = JsonSerializer.Serialize(data, _json);
        var tmp = _path + ".tmp";
        await File.WriteAllTextAsync(tmp, json);
        if (File.Exists(_path)) File.Delete(_path);
        File.Move(tmp, _path);
    }

    // CRUD operations

    public async Task<Employee> AddAsync(Employee employee)
    {
        if (employee is null) throw new ValidationException("Employee is null.");
        employee.Name = employee.Name?.Trim() ?? "";
        employee.Position  = employee.Position?.Trim()  ?? "";
        if (string.IsNullOrWhiteSpace(employee.Name)) throw new ValidationException("Name is required.");
        if (string.IsNullOrWhiteSpace(employee.Position))  throw new ValidationException("Position is required.");

        var employees = await LoadAsync();

        employee.Id = employees.Count > 0 ? employees.Max(x => x.Id) + 1 : 1;
        employees.Add(employee);

        await SaveAsync(employees);
        return employee;
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        var employees = await LoadAsync();
        return employees.FirstOrDefault(e => e.Id == employeeId);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await LoadAsync();
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        if (employee is null) throw new ValidationException("Employee is null.");

        var employees = await LoadAsync();
        var existingEmployee = employees.FirstOrDefault(e => e.Id == employee.Id);
        if (existingEmployee is null) throw new KeyNotFoundException($"Employee with Id {employee.Id} not found.");

        existingEmployee.Name = employee.Name?.Trim() ?? existingEmployee.Name;
        existingEmployee.Position = employee.Position?.Trim() ?? existingEmployee.Position;
        existingEmployee.Salary = employee.Salary;

        await SaveAsync(employees);
        return existingEmployee;
    }

    public async Task<bool> DeleteAsync(int employeeId)
    {
        var employees = await LoadAsync();
        var employeeToRemove = employees.FirstOrDefault(e => e.Id == employeeId);
        if (employeeToRemove is null) return false;

        employees.Remove(employeeToRemove);
        await SaveAsync(employees);
        return true;
    }

}