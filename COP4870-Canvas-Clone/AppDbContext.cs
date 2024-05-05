using System;
using Microsoft.EntityFrameworkCore;

namespace COP4870_Canvas_Clone
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        //public DbSet<CourseEnrollment> CourseEnrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User Id=postgres.lwifmvvzmctsjqgrosgv;Password=FSUNole4401!;Server=aws-0-us-west-1.pooler.supabase.com;Port=5432;Database=postgres;");
        }
    }
}

