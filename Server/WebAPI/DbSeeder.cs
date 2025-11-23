using System.Text.Json;
using EfcRepositories;
using Entities;
using Microsoft.EntityFrameworkCore; 
using AppDbContext = EfcRepositories.AppContext;

// Load initial data from old JSON files (users.json, posts.json, comments.json),
// so the new EF Core database has the same data as the previous FileRepositories/CLI.

namespace WebAPI;

public static class DbSeeder
{
    // Simple records matching JSON -> we'll map them to entities
    private record UserSeed(int Id, string Username, string Password);
    private record PostSeed(int Id, string Title, string Body, int UserId);
    private record CommentSeed(int Id, string Body, int UserId, int PostId);

    public static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        // If there are already users -> assume the database is seeded
        if (await ctx.Users.AnyAsync())
            return;

        var dataDir = Path.Combine(env.ContentRootPath, "data");

        var users = await LoadAsync<UserSeed>(Path.Combine(dataDir, "users.json"));
        var posts = await LoadAsync<PostSeed>(Path.Combine(dataDir, "posts.json"));
        var comments = await LoadAsync<CommentSeed>(Path.Combine(dataDir, "comments.json"));

        // USERS
        ctx.Users.AddRange(users.Select(u => new User
        {
            Id = u.Id,
            Username = u.Username,  // note: in the entity you probably have UserName
            Password = u.Password
        }));

        // POSTS – ignore LikedByUserIds from JSON, if you want to store them in DB, we'll do a separate step
        ctx.Posts.AddRange(posts.Select(p => new Post
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId
        }));

        // COMMENTS
        ctx.Comments.AddRange(comments.Select(c => new Comment
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId
        }));

        await ctx.SaveChangesAsync();
    }

    private static async Task<List<T>> LoadAsync<T>(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        var data = JsonSerializer.Deserialize<List<T>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return data ?? new List<T>();
    }
}
