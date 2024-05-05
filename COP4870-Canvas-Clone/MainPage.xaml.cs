using Microsoft.Maui.Controls;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COP4870_Canvas_Clone;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        LoadStudentsAsync();
    }

    private string connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true";


    private async void LoadStudentsAsync()
    {
        List<Student> students = new List<Student>();
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT id, name, email FROM students", conn)) // Ensure the correct column index or name for email
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student(
                                reader.GetInt32(0),   // ID
                                reader.GetString(1),  // Name
                                reader.GetString(2)   // Email
                            ));
                        }
                    }
                }
                StudentPicker.ItemsSource = students;
                StudentPicker.ItemDisplayBinding = new Binding("Name");

                if (students.Count > 0)
                {
                    StudentPicker.SelectedIndex = 0; // Set the first student as selected by default
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading students: {ex.Message}", "OK");
        }
    }


    private async void StudentPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (StudentPicker.SelectedIndex == -1) return;

        var selectedStudent = (Student)StudentPicker.SelectedItem;
        LoadCoursesForStudent(selectedStudent.Id);
    }

    private async void LoadCoursesForStudent(int studentId)
    {
        List<CourseOverviewModel> courses = new List<CourseOverviewModel>();
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = $@"
                SELECT c.id, c.name,
                       STRING_AGG(m.name, ', ') AS Modules,
                       STRING_AGG(a.name, ', ') AS Assignments
                FROM courses c
                JOIN student_courses sc ON c.id = sc.course_id AND sc.student_id = {studentId}
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
                            courses.Add(new CourseOverviewModel
                            {
                                Name = reader.GetString(1),
                                ModulesNames = reader.IsDBNull(2) ? "No modules" : reader.GetString(2),
                                AssignmentsNames = reader.IsDBNull(3) ? "No assignments" : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            CoursesCollectionView.ItemsSource = courses;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading courses for student: {ex.Message}", "OK");
        }
    }
}
