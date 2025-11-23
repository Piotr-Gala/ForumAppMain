using Microsoft.EntityFrameworkCore;

using Entities;


namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=C:\Users\piter\DNP1\dotnet\ForumApp\Server\EfcRepositories\app.db");
    }
}

