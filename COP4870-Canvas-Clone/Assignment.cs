using System;
using System.ComponentModel.DataAnnotations;  // Import for annotations

public class Assignment
{
    [Key]  // This annotation helps EF recognize this as the primary key
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DueDate { get; set; }

    // Navigation property
    //public Module Module { get; set; }
}
