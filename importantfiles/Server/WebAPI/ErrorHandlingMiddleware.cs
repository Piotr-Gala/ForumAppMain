using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts; // DomainException + derived

namespace WebAPI;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex) // our domain exceptions
        {
            var (status, title) = ex switch
            {
                EntityNotFoundException => (HttpStatusCode.NotFound, "Not Found"),
                DuplicateEntityException => (HttpStatusCode.Conflict, "Conflict"),
                ValidationException     => (HttpStatusCode.BadRequest, "Validation Error"),
                _                       => (HttpStatusCode.BadRequest, "Domain Error")
            };

            var problem = new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problem.Status ?? (int)status;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex) // everything else
        {
            var problem = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Server Error",
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = problem.Status.Value;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
