# ForumApp - AI Coding Agent Instructions

## Architecture Overview

This is a **layered C# console application** (.NET 10.0) implementing a forum system with Users, Posts, and Comments. The solution uses **Repository Pattern with Interface Segregation**.

### Key Layers (Server/ folder)
- **Entities/**: POCOs (User, Post, Comment) with simple properties
- **RepositoryContracts/**: Repository interfaces (`IUserRepository`, `IPostRepository`, `ICommentRepository`) + domain exceptions
- **FileRepositories/**: JSON file-based persistence with atomic writes (`.tmp` files)
- **InMemoryRepositories/**: Alternative in-memory implementations for testing/prototyping
- **CLI/**: Console UI with view classes organized by feature (ManageUsers/, ManagePosts/)

### Data Flow Pattern
1. **Program.cs** instantiates concrete repositories (FileRepositories by default)
2. **CliApp** receives repositories via constructor injection
3. **View classes** receive specific repositories they need (e.g., CreatePostView needs both IPostRepository and IUserRepository)
4. Views call repository methods directly—no service layer exists

## Critical Conventions

### Repository Implementation Rules
- **ID generation**: `Max(x => x.Id) + 1` or 1 if empty (see UserFileRepository.AddAsync)
- **Atomic file writes**: Always write to `.tmp` file first, then Move (PostFileRepository.SaveAsync pattern)
- **GetManyAsync returns IQueryable**: File repos use `ConfigureAwait(false).GetAwaiter().GetResult()` to load synchronously, then return `.AsQueryable()` on the list
- **Domain exceptions**: Throw `ValidationException`, `EntityNotFoundException`, or `DuplicateEntityException` from RepositoryContracts namespace—never generic exceptions

### Exception Handling
Central try-catch in **CliApp.RunAsync()** catches all DomainException types and displays user-friendly messages:
```csharp
catch (ValidationException ex) { Console.WriteLine($"[Input error] {ex.Message}"); }
catch (EntityNotFoundException ex) { Console.WriteLine($"[Not found] {ex.Message}"); }
```

### UI Patterns
- **ConsoleUi helpers** (CLI/Common/ConsoleUi.cs): Use `ReadInt()`, `ReadNonEmpty()`, `ReadOptional()` for input
- **View structure**: Each feature has a dedicated view class (CreatePostView, DeleteUserView, etc.)
- **Navigation**: Menu-driven with ShowAsync() methods—views instantiate nested views when needed

### Project References
- **CLI.csproj** references: Entities, FileRepositories, RepositoryContracts
- **FileRepositories.csproj** references: Entities, RepositoryContracts
- **InMemoryRepositories.csproj** references: Entities, RepositoryContracts
- All projects use `<ImplicitUsings>enable</ImplicitUsings>` and `<Nullable>enable</Nullable>`

## Common Tasks

### Adding a New Entity
1. Create POCO in **Entities/** (e.g., `Tag.cs`)
2. Define interface in **RepositoryContracts/** (e.g., `ITagRepository.cs`)
3. Implement in **FileRepositories/** following atomic-write pattern from existing repos
4. Optionally implement in **InMemoryRepositories/** for testing
5. Update **Program.cs** to instantiate and pass to CliApp
6. Add view classes in **CLI/UI/ManageTags/** folder

### Building & Running
```powershell
# Build entire solution
dotnet build ForumApp.sln

# Run CLI (from CLI project directory)
cd Server/CLI
dotnet run

# Data files (users.json, posts.json, comments.json) are created in CLI/bin/Debug/net10.0/
```

### Cross-Entity Queries
Use LINQ on `GetManyAsync()` results. Example from **ListPostsView.cs**:
```csharp
var usersById = _users.GetManyAsync().ToDictionary(u => u.Id, u => u.Username);
```
Example from **SinglePostView.cs**:
```csharp
var comments = _comments.GetManyAsync()
    .Where(c => c.PostId == post.Id)
    .OrderBy(c => c.Id);
```

### Validation Pattern
Validate in repository Add/Update methods (not in views):
```csharp
if (post is null) throw new ValidationException("Post is null.");
post.Title = post.Title?.Trim() ?? "";
if (string.IsNullOrWhiteSpace(post.Title)) throw new ValidationException("Title is required.");
```

## Architecture Decisions

**Why no service layer?** This is a learning project demonstrating Repository Pattern. Views call repositories directly to keep focus on persistence abstractions.

**Why IQueryable from file repos?** Maintains interface compatibility with InMemoryRepositories while allowing LINQ composition in consuming code. Trade-off: file repos load full dataset into memory.

**Why separate FileRepositories vs InMemoryRepositories?** Demonstrates dependency injection—swap implementations in Program.cs without changing any other code.
