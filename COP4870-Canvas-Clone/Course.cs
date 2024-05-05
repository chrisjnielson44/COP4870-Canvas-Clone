using System.Collections.Generic; // Ensure you have this for using List<T>

namespace COP4870_Canvas_Clone
{
    public class Course
    {
        public int Id { get; set; }  // Ensure this property exists
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        // Ensure the constructor matches these properties
        public Course(int id, string name, string code, string description)
        {
            Id = id;
            Name = name;
            Code = code;
            Description = description;
        }

        // If you don't always need all properties, consider using an empty constructor or initializing with default values
        public Course() { }

        // Optional: Constructor without id for when you're creating a new course that doesn't have an id yet
        public Course(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
        }

        public Course(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
