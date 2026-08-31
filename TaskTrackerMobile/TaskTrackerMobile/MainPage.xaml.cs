using TaskTrackerMobile.Models;
using TaskTrackerMobile.Services;

namespace TaskTrackerMobile;

public partial class MainPage : ContentPage
{
    private readonly TaskApiService apiService = new();

    public MainPage()
    {
        InitializeComponent();

        PriorityPicker.ItemsSource = Enum.GetValues<TaskPriority>();
        PriorityPicker.SelectedItem = TaskPriority.Medium;
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            return;
        }

        var task = new TaskItem
        {
            Title = TaskEntry.Text,
            Description = "",
            DueDate = null,
            IsCompleted = false,
            Priority = (TaskPriority)PriorityPicker.SelectedItem
        };

        try
        {
            await apiService.AddTaskAsync(task);

            TaskEntry.Text = "";
            PriorityPicker.SelectedItem = TaskPriority.Medium;

            var tasks = await apiService.GetTasksAsync();
            TaskList.ItemsSource = tasks;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var tasks = await apiService.GetTasksAsync();
            TaskList.ItemsSource = tasks;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox checkBox || checkBox.BindingContext is not TaskItem task)
        {
            return;
        }
        task.IsCompleted = e.Value;
        try
        {
            await apiService.UpdateTaskAsync(task);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnDeleteTaskClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not TaskItem task)
        {
            return;
        }
        try
        {
            await apiService.DeleteTaskAsync(task.Id);
            var tasks = await apiService.GetTasksAsync();
            TaskList.ItemsSource = tasks;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}