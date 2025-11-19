using ApiContracts.Users;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<List<UserDto>> GetAllAsync();
    public Task UpdateUserAsync(int id, UpdateUserDto request);
    public Task DeleteAsync(int id);
    public Task<UserDto> GetByIdAsync(int id);
    
    // more methods...
}