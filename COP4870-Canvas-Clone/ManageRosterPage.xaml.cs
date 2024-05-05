using Microsoft.Maui.Controls;
using Npgsql;
using System;

namespace COP4870_Canvas_Clone;

public partial class ManageRosterPage : ContentPage
{
    public ManageRosterPage()
    {
        InitializeComponent();
        LoadCourses();
        LoadStudents();
        LoadCoursesWithStudents();
    }

    string connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Username=postgres.lwifmvvzmctsjqgrosgv;Password=VRkBxJ8nH71BXj92;Port=5432;Database=postgres;SSL Mode=Require;Trust Server Certificate=true";


    private async void LoadStudents()
    {
        List<Student> students = new List<Student>();
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT id, name, email FROM students", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student(
                                reader.GetInt32(0),  // id
                                reader.GetString(1), // name
                                reader.GetString(2)  // email
                            ));
                        }
                    }
                }
            }
            StudentPicker.ItemsSource = students;
            StudentPicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading students: {ex.Message}", "OK");
        }
    }


    private async void LoadCourses()
    {
        List<Course> courses = new List<Course>();
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT id, name FROM courses", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
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

    private async void OnStudentSelected(object sender, EventArgs e)
    {
        var selectedStudent = StudentPicker.SelectedItem as Student;
        if (selectedStudent == null) return;

        try
        {
            // Assuming you have a view or a complex query that fetches courses, modules, and assignments
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT course_name, module_id, module_name, assignment_id, assignment_name FROM student_details_view WHERE student_id = @studentId", conn))
                {
                    cmd.Parameters.AddWithValue("studentId", selectedStudent.Id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while fetching student details: {ex.Message}", "OK");
        }
    }

    private async void OnAddStudentToCourse(object sender, EventArgs e)
    {
        var selectedStudent = StudentPicker.SelectedItem as Student;
        var selectedCourse = CoursePicker.SelectedItem as Course;
        if (selectedStudent == null || selectedCourse == null)
        {
            await DisplayAlert("Selection Error", "Please select both a student and a course.", "OK");
            return;
        }

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                // Check if the student is already enrolled in the selected course
                using (var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM student_courses WHERE student_id = @studentId AND course_id = @courseId", conn))
                {
                    checkCmd.Parameters.AddWithValue("studentId", selectedStudent.Id);
                    checkCmd.Parameters.AddWithValue("courseId", selectedCourse.Id);
                    var count = (long)await checkCmd.ExecuteScalarAsync();
                    if (count > 0)
                    {
                        await DisplayAlert("Duplicate Error", "Student is already enrolled in this course.", "OK");
                        return;
                    }
                }

                // Insert new enrollment record
                using (var enrollCmd = new NpgsqlCommand("INSERT INTO student_courses (student_id, course_id) VALUES (@studentId, @courseId)", conn))
                {
                    enrollCmd.Parameters.AddWithValue("studentId", selectedStudent.Id);
                    enrollCmd.Parameters.AddWithValue("courseId", selectedCourse.Id);
                    await enrollCmd.ExecuteNonQueryAsync();
                    await DisplayAlert("Success", "Student enrolled successfully.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while enrolling student: {ex.Message}", "OK");
        }
    }

    private async void LoadCoursesWithStudents()
    {

        List<CourseViewModel> coursesWithStudents = new List<CourseViewModel>();

        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = @"
            SELECT c.id, c.name, s.id, s.name, s.email
            FROM courses c
            LEFT JOIN student_courses sc ON c.id = sc.course_id
            LEFT JOIN students s ON s.id = sc.student_id
            ORDER BY c.name, s.name";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        CourseViewModel lastCourse = null;
                        while (reader.Read())
                        {
                            int courseId = reader.GetInt32(0);
                            string courseName = reader.GetString(1);

                            if (lastCourse == null || lastCourse.Id != courseId)
                            {
                                lastCourse = new CourseViewModel
                                {
                                    Id = courseId,
                                    Name = courseName,
                                    EnrolledStudents = new List<Student>()
                                };
                                coursesWithStudents.Add(lastCourse);
                            }

                            if (!reader.IsDBNull(2)) // Check if student data is present
                            {
                                lastCourse.EnrolledStudents.Add(new Student(
                                    reader.GetInt32(2),    // id
                                    reader.GetString(3),   // name
                                    reader.GetString(4)    // email
                                ));
                            }
                        }
                    }
                }
                CoursesCollectionView.ItemsSource = null; // Clear the ItemsSource

                CoursesCollectionView.ItemsSource = coursesWithStudents;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Database Error", $"An error occurred while loading courses: {ex.Message}", "OK");
        }
    }

  


    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        LoadCoursesWithStudents();  // This method will refresh the data
    }



}
