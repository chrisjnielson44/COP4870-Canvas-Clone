using System;
namespace COP4870_Canvas_Clone
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Student> EnrolledStudents { get; set; } = new List<Student>();

        public string DisplayText => $"{Name} - Enrolled Students: {EnrolledStudents.Count}";
    }

}

