using ApiContracts.Users;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<List<UserDto>> GetAllAsync();
    public Task UpdateUserAsync(int id, UpdateUserDto request);

    // NEW:
    Task DeleteAsync(int id);
    
    // more methods...
}