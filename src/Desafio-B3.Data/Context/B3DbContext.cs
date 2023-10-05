using ImplementandoRabiitMQ.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImplementandoRabiitMQ.Data.Context
{
    public class B3DbContext : DbContext
    {
        public B3DbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<Tarefa> Tarefas { get; set; }
     
    }
}
