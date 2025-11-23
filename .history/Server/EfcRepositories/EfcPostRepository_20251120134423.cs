using System.Linq;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;


namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;  // EF DbContext

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;      // injected context
    }

    // ADD POST (DB)
    public async Task<Post> AddAsync(Post post)
    {
        // validate fields
        if (string.IsNullOrWhiteSpace(post.Title))
            throw new ValidationException("Title cannot be empty.");
        if (string.IsNullOrWhiteSpace(post.Body))
            throw new ValidationException("Body cannot be empty.");

        // check duplicate username
        bool exists = await ctx.Posts
            .AnyAsync(p => p.Title.ToLower() == post.Title.ToLower()); // DB check

        if (exists)
            throw new ValidationException($"Title '{post.Title}' already exists.");

        await ctx.Posts.AddAsync(post); // track new entity
        await ctx.SaveChangesAsync();  // write to SQLite
        return post;                  // Id now set by DB
    }

    // UPDATE POST (DB)
    public async Task UpdateAsync(Post post)
    {
        // post exists?
        bool exists = await ctx.Posts
            .AnyAsync(p => p.Id == post.Id);      // PK lookup

        if (!exists)
            throw new EntityNotFoundException($"Post with ID {post.Id} not found.");

        // duplicate username on update
        bool duplicate = await ctx.Posts
            .AnyAsync(p => p.Id != post.Id &&
                           p.Title.ToLower() == post.Title.ToLower()); // DB uniqueness

        if (duplicate)
            throw new ValidationException($"Title '{post.Title}' already exists.");

        ctx.Posts.Update(post);         // mark as modified
        await ctx.SaveChangesAsync();   // commit changes
    }

    // DELETE POST (DB)
    public async Task DeleteAsync(int id)
    {
        var existing = await ctx.Posts
            .SingleOrDefaultAsync(p => p.Id == id);   // single row query
        if (existing is null)
            throw new EntityNotFoundException($"Post with ID {id} not found.");

        ctx.Posts.Remove(existing);    // mark for delete
        await ctx.SaveChangesAsync(); // commit delete
    }

    // GET SINGLE Post (DB)
    public async Task<Post> GetSingleAsync(int id)
    {
        var post = await ctx.Posts
            .SingleOrDefaultAsync(p => p.Id == id);   // PK lookup

        if (post is null)
            throw new EntityNotFoundException($"Post with ID {id} not found.");
        return post;     // tracked entity
    }


    // GET MANY POSTS (DB)
    public IQueryable<Post> GetManyAsync()
        => ctx.Posts.AsQueryable();
}