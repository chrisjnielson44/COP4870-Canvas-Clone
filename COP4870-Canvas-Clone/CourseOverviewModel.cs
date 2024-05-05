using System;
namespace COP4870_Canvas_Clone
{
    public class CourseOverviewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StudentCount { get; set; }  // Changed to string to directly store the formatted count
        public string ModulesNames { get; set; }  // To store concatenated module names
        public string AssignmentsNames { get; set; } // This will be converted to a string for display purposes.
        public List<Module> Modules { get; set; }

        public CourseOverviewModel()
        {
            Modules = new List<Module>();
        }
    }


}

