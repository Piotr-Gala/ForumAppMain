using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        bool exists = await ctx.Users.AnyAsync(u => u.Id == user.Id);
        if (!exists)
        {
            throw new EntityNotFoundException($"User with id {user.Id} not found");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existing is null)
        {
            throw new EntityNotFoundException($"User with id {id} not found");
        }

        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new EntityNotFoundException($"User with id {id} not found");
        }

        return user;
    }

    public IQueryable<User> GetManyAsync()
    {
        return ctx.Users.AsQueryable();
    }
}
