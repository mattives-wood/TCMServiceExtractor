using Domain;
using Domain.Meta;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;

namespace Data
{
    public class MetadataContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public MetadataContext(DbContextOptions<MetadataContext> options) : base(options) { }

        public DbSet<Metadata> Metadatas { get; set; }
        public DbSet<BH> BH { get; set; }
        public DbSet<BHFS> BHFS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("Development") == "true")
            {
                optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metadata>().Property(o => o.ServiceDateTime).HasPrecision(3);
            modelBuilder.Entity<Metadata>().Property(o => o.StartDate).HasPrecision(3);
            modelBuilder.Entity<Metadata>().Property(o => o.EndDate).HasPrecision(3);
            modelBuilder.Entity<Metadata>().Property(o => o.EffectiveDate).HasPrecision(3);
            base.OnModelCreating(modelBuilder);
        }
    }
}
