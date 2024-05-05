using System;
using Microsoft.Maui.Controls;
using Npgsql;

namespace COP4870_Canvas_Clone;



public partial class AddModulesPage : ContentPage
{
    public AddModulesPage()
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
                Console.WriteLine("Connected to database successfully.");  // Debug output

                using (var cmd = new NpgsqlCommand("SELECT id, name, code, description FROM courses", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Code = reader.GetString(2),
                                Description = reader.GetString(3)
                            });
                            Console.WriteLine("Loaded course: " + reader.GetString(1));  // Debug output
                        }
                    }
                }
            }

            if (courses.Count == 0)
                Console.WriteLine("No courses found.");  // Debug output
            else
                CoursePicker.ItemsSource = courses;

            CoursePicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Database Error: " + ex.Message);  // Debug output
            await DisplayAlert("Database Error", $"An error occurred while loading courses: {ex.Message}", "OK");
        }
    }



    private async void OnAddModuleClicked(object sender, EventArgs e)
    {
        var selectedCourse = CoursePicker.SelectedItem as Course;
        if (selectedCourse == null)
        {
            await DisplayAlert("Error", "Please select a course.", "OK");
            return;
        }

        string moduleName = ModuleNameEntry.Text;
        if (string.IsNullOrWhiteSpace(moduleName))
        {
            await DisplayAlert("Validation Error", "Please enter a module title.", "OK");
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection("Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true"))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO modules (course_id, name, description) VALUES (@course_id, @name, '')", conn))
                {
                    cmd.Parameters.AddWithValue("course_id", selectedCourse.Id);
                    cmd.Parameters.AddWithValue("name", moduleName);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            await DisplayAlert("Success", "Module added successfully to the course.", "OK");
            ModuleNameEntry.Text = string.Empty;  // Optionally clear the entry field after successful addition
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

}
