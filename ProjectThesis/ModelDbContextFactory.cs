using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProjectThesis.Models;

namespace ProjectThesis
{
    public class ModelContextFactory : IDesignTimeDbContextFactory<ModelContext>
    {
        public ModelContext CreateDbContext(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseOracle(connectionString);
            return new ModelContext(optionsBuilder.Options);
        }
    }
}
