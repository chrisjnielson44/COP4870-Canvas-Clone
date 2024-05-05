using System.Collections.Generic;
using System.Linq;

namespace COP4870_Canvas_Clone
{
    public class Student
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Course> EnrolledCourses { get; set; } = new List<Course>();

        public
            Student(int id, string name, string email)  // Constructor now includes id
        {
            Id = id;
            Name = name;
            Email = email;
        }


        public string GetFormattedCourseList()
        {
            return string.Join(", ", EnrolledCourses.Select(c => c.Name));
        }
    }
}
