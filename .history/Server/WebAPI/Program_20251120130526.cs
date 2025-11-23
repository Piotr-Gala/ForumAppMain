using FileRepositories;
using RepositoryContracts;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ErrorHandlingMiddleware>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dataDir = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataDir);

builder.Services.AddScoped<IUserRepository>(_ =>
    new UserFileRepository(Path.Combine(dataDir, "users.json")));
builder.Services.AddScoped<IPostRepository>(_ =>
    new PostFileRepository(Path.Combine(dataDir, "posts.json")));
builder.Services.AddScoped<ICommentRepository>(_ =>
    new CommentFileRepository(Path.Combine(dataDir, "comments.json")));
builder.Services.AddScoped<IEmployeeRepository>(_ =>
    new EmployeeFileRepository(Path.Combine(dataDir, "employees.json")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
