//using System;
//namespace COP4870_Canvas_Clone
//{
//    public static class SchoolRepository
//    {
//        // Adds a module to a student within a specific course
//        public static void AssignModuleToStudent(Student student, Course course, Module module)
//        {
//            if (student.CourseModules.ContainsKey(course))
//            {
//                if (!student.CourseModules[course].Contains(module))
//                {
//                    student.CourseModules[course].Add(module);
//                }
//            }
//            else
//            {
//                student.CourseModules[course] = new List<Module> { module };
//            }
//        }
//    }
//}

