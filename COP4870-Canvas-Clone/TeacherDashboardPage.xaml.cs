using System;
using Microsoft.Maui.Controls;
using Npgsql;

namespace COP4870_Canvas_Clone;

public partial class TeacherDashboardPage : ContentPage
{
    public TeacherDashboardPage()
    {
        InitializeComponent();
        LoadCoursesWithStudents();
    }

    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        // Navigate to Add Student Page
        await Navigation.PushAsync(new AddStudentPage());
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        // Navigate to Add Course Page
        await Navigation.PushAsync(new AddCoursePage());
    }

    private async void OnManageRostersClicked(object sender, EventArgs e)
    {
        // Navigate to Manage Rosters Page
        await Navigation.PushAsync(new ManageRosterPage());
    }

    private async void OnAddModulesClicked(object sender, EventArgs e)
    {
        // Navigate to Add Modules Page
        await Navigation.PushAsync(new AddModulesPage());
    }

    private async void OnAddAssignmentsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewAddAssignmentPage());
    }

    string connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true";


    private async void LoadCoursesWithStudents()
    {
        List<CourseOverviewModel> courses = new List<CourseOverviewModel>();
        
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = @"
            SELECT c.id, c.name, COUNT(DISTINCT sc.student_id) AS student_count,
                   STRING_AGG(DISTINCT m.name, ', ') AS modules_names,
                   STRING_AGG(DISTINCT a.name, ', ') AS assignments_names
            FROM courses c
            LEFT JOIN student_courses sc ON c.id = sc.course_id
            LEFT JOIN modules m ON c.id = m.course_id
            LEFT JOIN assignments a ON m.id = a.module_id
            GROUP BY c.id
            ORDER BY c.name";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var course = new CourseOverviewModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                StudentCount = reader.GetInt32(2).ToString(), // Convert student count to string here
                                ModulesNames = reader.IsDBNull(3) ? "No modules" : reader.GetString(3),
                                AssignmentsNames = reader.IsDBNull(4) ? "No assignments" : reader.GetString(4)
                            };
                            courses.Add(course);
                        }
                    }
                }
                CoursesCollectionView.ItemsSource = null; // Clear the ItemsSource
                CoursesCollectionView.ItemsSource = courses; // Assuming CoursesCollectionView is a CollectionView in your XAML
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading course overviews: {ex.Message}", "OK");
        }
    }




    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        LoadCoursesWithStudents();  // This method will refresh the data
    }

}
