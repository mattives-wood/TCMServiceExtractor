using Domain.Meta;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;

namespace Data;
public class MetaContext : DbContext
{
    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

    public MetaContext(DbContextOptions<MetaContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Metadata> Metadatas { get; set; }

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
