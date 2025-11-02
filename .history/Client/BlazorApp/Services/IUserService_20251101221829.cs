using ApiContracts.Users;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<List<UserDto>> GetAllAsync();
    public Task UpdateUserAsync(int id, UpdateUserDto request);

    // more methods...
}