using System;
using Microsoft.Maui.Controls;
using Npgsql;

namespace COP4870_Canvas_Clone;

public partial class AddStudentPage : ContentPage
{
    private string connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true";

    public AddStudentPage()
    {
        InitializeComponent();
    }

    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        string studentName = StudentNameEntry.Text;
        string studentEmail = StudentEmailEntry.Text;

        if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentEmail))
        {
            await DisplayAlert("Validation Error", "Please enter both name and email.", "OK");
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO students (name, email) VALUES (@name, @email)", conn))
                {
                    cmd.Parameters.AddWithValue("name", studentName);
                    cmd.Parameters.AddWithValue("email", studentEmail);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            await DisplayAlert("Success", "Student added successfully.", "OK");

            // Clear the fields after successful addition
            StudentNameEntry.Text = string.Empty;
            StudentEmailEntry.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
