using System;
namespace COP4870_Canvas_Clone
{
    public static class StudentRepository
    {
        public static List<Student> Students { get; private set; } = new List<Student>();

        public static void AddStudent(Student student)
        {
            Students.Add(student);
        }
    }
}

