using Formula1.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Formula1.Persistence
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // Im DatenbankContext verwaltete Collections

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Race> Races { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration["ConnectionStrings:DefaultConnection"];

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
