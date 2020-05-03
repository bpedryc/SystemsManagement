using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectThesis.Models
{
    public partial class ModelContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        {
        }

        //public override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<User>()
        //        .Property(property => property.UserID)
        //        .ValueGeneratedOnAdd();
        //}
    }
}
