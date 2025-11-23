using System.Linq;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;


namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;  // EF DbContext

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;      // injected context
    }

    // ADD COMMENT (DB)
    public async Task<Comment> AddAsync(Comment comment)
    {
        // validate fields
        if (string.IsNullOrWhiteSpace(comment.Body))
            throw new ValidationException("Body cannot be empty.");

        await ctx.Comments.AddAsync(comment); // track new entity
        await ctx.SaveChangesAsync();  // write to SQLite
        return comment;                  // Id now set by DB
    }

    // UPDATE COMMENT (DB)
    public async Task UpdateAsync(Comment comment)
    {
        // comment exists?
        bool exists = await ctx.Comments
            .AnyAsync(c => c.Id == comment.Id);      // PK lookup

        if (!exists)
            throw new EntityNotFoundException($"Comment with ID {comment.Id} not found.");

        ctx.Comments.Update(comment);         // mark as modified
        await ctx.SaveChangesAsync();   // commit changes
    }

    // DELETE COMMENT (DB)
    public async Task DeleteAsync(int id)
    {
        var existing = await ctx.Comments
            .SingleOrDefaultAsync(c => c.Id == id);   // single row query

        if (existing is null)
            throw new EntityNotFoundException($"Comment with ID {id} not found.");

        ctx.Comments.Remove(existing);    // mark for delete
        await ctx.SaveChangesAsync(); // commit delete
    }

    // GET COMMENT BY ID (DB)
    public async Task<Comment> GetSingleAsync(int id)
    {
        var comment = await ctx.Comments
            .AsNoTracking()               // read-only
            .SingleOrDefaultAsync(c => c.Id == id); // PK lookup

        if (comment is null)
            throw new EntityNotFoundException($"Comment with ID {id} not found.");

        return comment;
    }

    // GET ALL COMMENTS (DB)
    public IQueryable<Comment> GetManyAsync()
    {
        return ctx.Comments
            .AsNoTracking()           // read-only
            .AsQueryable();           // return IQueryable for callers to compose
    } 

    

}