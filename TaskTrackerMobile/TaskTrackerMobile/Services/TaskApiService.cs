using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTrackerMobile.Models;

namespace TaskTrackerMobile.Services;

public class TaskApiService
{
    private readonly HttpClient httpClient;

    private readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public TaskApiService()
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5071")
        };
    }

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        var tasks = await httpClient.GetFromJsonAsync<List<TaskItem>>(
            "/tasks",
            jsonOptions
        );

        return tasks ?? new List<TaskItem>();
    }

    public async Task<TaskItem?> AddTaskAsync(TaskItem task)
    {
        var response = await httpClient.PostAsJsonAsync(
            "/tasks",
            task,
            jsonOptions
        );

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TaskItem>(jsonOptions);
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        var response = await httpClient.PutAsJsonAsync(
            $"/tasks/{task.Id}",
            task,
            jsonOptions
        );

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/tasks/{id}");
        response.EnsureSuccessStatusCode();
    }
}