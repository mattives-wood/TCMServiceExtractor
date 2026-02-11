using System;
using System.Linq;

using Domain.Financials;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data;
public class FinancialsContext : DbContext
{
    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    public FinancialsContext(DbContextOptions<FinancialsContext> options) : base(options) { }

    public DbSet<Client> clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("Development") == "true")
        {
            optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(() => GetAdjustmentsForContact(default)).HasName("getAdjustmentsForContact");
        modelBuilder.Entity<Adjustment>().HasNoKey();

        modelBuilder.HasDbFunction(() => GetServiceLinesForClient(default)).HasName("getServiceLinesForClient");
        modelBuilder.Entity<ServiceLine>().HasNoKey();

        modelBuilder.HasDbFunction(() => GetClaimsForContact(default)).HasName("getClaimsForContact");
        modelBuilder.Entity<Claim>().HasNoKey();

        modelBuilder.HasDbFunction(() => GetLastStatementForClient(default)).HasName("getLastStatementDueByClient");
        modelBuilder.Entity<LastStatement>().HasNoKey();

        modelBuilder.HasDbFunction(() => GetPaymentsForContact(default)).HasName("getPaymentsForContact");
        modelBuilder.Entity<Payment>().HasNoKey();

        modelBuilder.HasDbFunction(() => GetServiceLineBalancesForClient(default)).HasName("getServiceLineBalances");
        modelBuilder.Entity<ServiceLineBalance>().HasNoKey();

        modelBuilder.HasDbFunction(() => GetRespPartyForClient(default)).HasName("getRespPartyForClient");
        modelBuilder.Entity<ResponsibleParty>().HasNoKey();

        modelBuilder.HasDbFunction(
            typeof(FinancialsContext).GetMethod(nameof(GetUnbilledCostsForClient),
            new[] { typeof(int) })!)
                    .HasName("getUnbilledCostsForClient");

        base.OnModelCreating(modelBuilder);
    }

    public IQueryable<Adjustment> GetAdjustmentsForContact(int contactId) =>
        FromExpression(() => GetAdjustmentsForContact(contactId));

    public IQueryable<ServiceLine> GetServiceLinesForClient(int clientId) =>
        FromExpression(() => GetServiceLinesForClient(clientId));

    public IQueryable<Claim> GetClaimsForContact(int contactId) =>
        FromExpression(() => GetClaimsForContact(contactId));

    public IQueryable<LastStatement> GetLastStatementForClient(int clientId) =>
        FromExpression(() => GetLastStatementForClient(clientId));

    public IQueryable<Payment> GetPaymentsForContact(int contactId) =>
        FromExpression(() => GetPaymentsForContact(contactId));

    public IQueryable<ServiceLineBalance> GetServiceLineBalancesForClient(int clientId) =>
        FromExpression(() => GetServiceLineBalancesForClient(clientId));

    public IQueryable<ResponsibleParty> GetRespPartyForClient(int clientId) =>
        FromExpression(() => GetRespPartyForClient(clientId));

    public static decimal GetUnbilledCostsForClient(int clientId)
        => throw new NotImplementedException();
}