    using System.Collections.Generic;

    namespace COP4870_Canvas_Clone
    {
        public class Module
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Assignment> Assignments { get; set; }  // List to hold assignments

            public Module(int id, string name)
            {
                Id = id;
                Name = name;
                Assignments = new List<Assignment>();  // Initialize the list of assignments
            }
        }
    }
