using ApiContracts.Users;
using System.Text.Json;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, JsonOpts)!;
    }

    public async Task<List<UserDto>> GetAllAsync()
    => await client.GetFromJsonAsync<List<UserDto>>("users", JsonOpts) ?? [];

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"users/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) throw new Exception(response);
    }

    // NEW: delete (masz w API)
    public async Task DeleteAsync(int id)
    {
        var resp = await client.DeleteAsync($"users/{id}");
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(txt);
    }
    
    // more methods...
}
