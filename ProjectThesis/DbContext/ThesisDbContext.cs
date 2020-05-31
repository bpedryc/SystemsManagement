using System;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.Models;

namespace ProjectThesis.ViewModels
{
    public class ThesisDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        
        public ThesisDbContext(DbContextOptions<ThesisDbContext> options) : base(options)
        {

        }

    }
}
