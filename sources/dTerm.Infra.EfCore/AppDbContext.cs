using dTerm.Core;
using dTerm.Infra.EfCore.SeederScripts;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace dTerm.Infra.EfCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProcessEntity> Consoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var baseDir = AppData.GetBaseDirectoryInfo();
            var dataSource = Path.Combine(baseDir.FullName, "App.db");

            optionsBuilder.UseSqlite($"Data Source='{dataSource}'");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessEntity>(entities =>
            {
                entities.ToTable("Consoles");

                entities.Property(e => e.Icon).IsRequired().HasMaxLength(ProcessEntity.IconMaxLength);
                entities.Property(e => e.Name).IsRequired().HasMaxLength(ProcessEntity.NameMaxLength);
                entities.Property(e => e.ProcessExecutablePath).IsRequired().HasMaxLength(ProcessEntity.ProcessExecutablePathMaxLength);
                entities.Property(e => e.ProcessStartupArgs).HasMaxLength(ProcessEntity.ProcessStartupArgsMaxLength);

                entities.HasData(DefaultConsolesSeeder.GetEntities());
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
