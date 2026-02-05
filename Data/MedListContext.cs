using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.MedList;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data;

public class MedListContext : DbContext
{
    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    public MedListContext(DbContextOptions<MedListContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }

    public DbSet<DrugLkp> DrugLkps { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<MedApplicationLkp> MedApplications { get; set; }

    public DbSet<MedExplanationLkp> MedExplanations { get; set; }

    public DbSet<Medication> Medications { get; set; }

    public DbSet<MedInfo> MedInfos { get; set; }

    public DbSet<Pharmacy> Pharmacies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("Development") == "true")
        {
            optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}