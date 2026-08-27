using TaskTrackerApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Task Tracker API is running!");

app.MapGet("/hello/{name}", (string name) =>
{
    return $"Hello, {name}!";
});

app.MapGet("/tasks", () =>
{
    var tasks = new List<TaskItem>
    {
        new TaskItem
        {
            Id = 1,
            Title = "Prepare for interview",
            Description = "Review C# and SQL",
            DueDate = DateTime.Today.AddDays(1),
            IsCompleted = false,
            Priority = "High"
        },

        new TaskItem
        {
            Id = 2,
            Title = "Finish Task Tracker",
            Description = "Build CRUD endpoints",
            DueDate = DateTime.Today.AddDays(2),
            IsCompleted = false,
            Priority = "Medium"
        }
    };

    return tasks;
});

app.Run();