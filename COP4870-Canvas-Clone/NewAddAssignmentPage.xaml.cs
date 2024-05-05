using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Npgsql;

namespace COP4870_Canvas_Clone;
public partial class NewAddAssignmentPage: ContentPage
{
    public NewAddAssignmentPage()
    {
        InitializeComponent();
        LoadCourses();
    }

    string connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true";


    private async void LoadCourses()
    {
        try
        {
            List<Course> courses = new List<Course>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT id, name FROM courses", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
            }
            CoursePicker.ItemsSource = courses;
            CoursePicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading courses: {ex.Message}", "OK");
        }
    }

    private async void OnCourseSelected(object sender, EventArgs e)
    {
        var selectedCourse = CoursePicker.SelectedItem as Course;
        if (selectedCourse == null) return;

        try
        {
            List<Module> modules = new List<Module>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT id, name FROM modules WHERE course_id = @courseId", conn))
                {
                    cmd.Parameters.AddWithValue("courseId", selectedCourse.Id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            modules.Add(new Module(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
            }
            ModulePicker.ItemsSource = modules;
            ModulePicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading modules: {ex.Message}", "OK");
        }
    }


    private void OnModuleSelected(object sender, EventArgs e)
    {
        // Optionally, you can handle additional logic when a module is selected
    }

    private async void OnAddAssignmentClicked(object sender, EventArgs e)
    {
        var selectedModule = ModulePicker.SelectedItem as Module;
        if (selectedModule == null)
        {
            await DisplayAlert("Error", "Please select a module first.", "OK");
            return;
        }

        string assignmentName = AssignmentNameEntry.Text;
        string assignmentDescription = AssignmentDescriptionEditor.Text;
        string dueDateString = AssignmentDueDateEntry.Text;

        DateTime dueDate;
        if (!DateTime.TryParseExact(dueDateString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
        {
            await DisplayAlert("Error", "Invalid date format. Please enter a date in MM/dd/yyyy format.", "OK");
            return;
        }

        if (dueDate < DateTime.Today)
        {
            await DisplayAlert("Error", "Due date cannot be in the past.", "OK");
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO assignments (module_id, name, description, due_date) VALUES (@moduleId, @name, @description, @dueDate)", conn))
                {
                    cmd.Parameters.AddWithValue("moduleId", selectedModule.Id);
                    cmd.Parameters.AddWithValue("name", assignmentName);
                    cmd.Parameters.AddWithValue("description", assignmentDescription);
                    cmd.Parameters.AddWithValue("dueDate", dueDate);  // assuming the database still expects a date type

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            await DisplayAlert("Success", "Assignment added successfully.", "OK");
            AssignmentNameEntry.Text = string.Empty;
            AssignmentDescriptionEditor.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

}