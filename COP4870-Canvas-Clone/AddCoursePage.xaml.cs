using System;
using Microsoft.Maui.Controls;
using Npgsql;

namespace COP4870_Canvas_Clone;

public partial class AddCoursePage : ContentPage
{
    private string connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true";

    public AddCoursePage()
    {
        InitializeComponent();
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        string courseName = CourseNameEntry.Text;
        string courseCode = CourseCodeEntry.Text;
        string courseDescription = CourseDescriptionEditor.Text;

        if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(courseCode))
        {
            await DisplayAlert("Validation Error", "Please enter both the course name and course code.", "OK");
            return;
        }

        // Validate course description length
        if (courseDescription.Length > 500)  // Assuming the max length is 500 characters
        {
            await DisplayAlert("Validation Error", "Course description is too long. Please limit to 500 characters.", "OK");
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO courses (name, code, description) VALUES (@name, @code, @description)", conn))
                {
                    cmd.Parameters.AddWithValue("name", courseName);
                    cmd.Parameters.AddWithValue("code", courseCode);
                    cmd.Parameters.AddWithValue("description", courseDescription);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            await DisplayAlert("Success", "Course added successfully.", "OK");

            // Clear the fields after successful addition
            CourseNameEntry.Text = string.Empty;
            CourseCodeEntry.Text = string.Empty;
            CourseDescriptionEditor.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
