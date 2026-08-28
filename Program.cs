using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Models;
using TaskTrackerApi.Data;

var builder = WebApplication.CreateBuilder(args);

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
    dbContext.Tasks.Add(task);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
});

app.Run();