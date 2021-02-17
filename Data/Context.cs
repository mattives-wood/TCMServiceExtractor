using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Intakes> Intakes { get; set; }
        public DbSet<ProgramLkp> Programs { get; set; }
        public DbSet<ProgressNotes> Notes { get; set; }
    }
}
