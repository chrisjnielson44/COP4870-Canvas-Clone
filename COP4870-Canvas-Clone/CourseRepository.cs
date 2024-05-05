using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace COP4870_Canvas_Clone
{
    public static class CourseRepository
    {
        public static List<Course> Courses { get; } = new List<Course>();

        public static void AddCourse(Course course)
        {
            Courses.Add(course);
        }
    }

}

