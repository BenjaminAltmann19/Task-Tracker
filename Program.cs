using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Models;
using TaskTrackerApi.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

var app = builder.Build();

app.MapGet("/", () => "Task Tracker API is running!");


app.MapGet("/tasks", async (TaskDbContext dbContext) =>
{
    return await dbContext.Tasks.ToListAsync();
});

app.MapPost("/tasks", async (TaskItem task, TaskDbContext dbContext) =>
{
    if (string.IsNullOrWhiteSpace(task.Title))
    {
        return Results.BadRequest("Title is required.");
    }

    dbContext.Tasks.Add(task);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id}", async (int id, TaskItem updatedTask, TaskDbContext dbContext) =>
{
    if (string.IsNullOrWhiteSpace(updatedTask.Title))
    {
        return Results.BadRequest("Title is required.");
    }

    var task = await dbContext.Tasks.FindAsync(id);

    if (task is null)
    {
        return Results.NotFound();
    }

    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.DueDate = updatedTask.DueDate;
    task.IsCompleted = updatedTask.IsCompleted;
    task.Priority = updatedTask.Priority;

    await dbContext.SaveChangesAsync();

    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", async (int id, TaskDbContext dbContext) =>
{
    var task = await dbContext.Tasks.FindAsync(id);

    if(task is null)
    {
        return Results.NotFound();
    }

    dbContext.Tasks.Remove(task);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/tasks/{id}", async (int id, TaskDbContext dbContext) =>
{
    var task = await dbContext.Tasks.FindAsync(id);

    if(task is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(task);
});

app.Run();