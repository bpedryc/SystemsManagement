using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProjectThesis.ViewModels;

namespace ProjectThesis
{
    public class ThesisDbContextFactory: IDesignTimeDbContextFactory<ThesisDbContext>
    {
        public ThesisDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ThesisDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseOracle(connectionString);
            return new ThesisDbContext(optionsBuilder.Options);
        }
    }
}
