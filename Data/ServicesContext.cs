using Domain.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;

namespace Data
{
    public class ServicesContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public ServicesContext(DbContextOptions<ServicesContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Contacts> Contacts { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Intakes> Intakes { get; set; }

        public DbSet<LocationLkp> Locations { get; set; }

        public DbSet<ProgramLkp> Programs { get; set; }

        public DbSet<ProgressNotes> Notes { get; set; }

        public DbSet<ServiceCodes> ServiceCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("Development") == "true")
            {
                optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contacts>().Property(o => o.ServDate).HasPrecision(3);
            base.OnModelCreating(modelBuilder);
        }
    }
}
