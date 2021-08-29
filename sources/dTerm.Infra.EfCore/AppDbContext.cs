using dTerm.Domain;
using dTerm.Infra.EfCore.SeederScripts;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace dTerm.Infra.EfCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<ShellProcess> Consoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var baseDir = AppStorage.GetBaseDirectoryInfo();
            var dataSource = Path.Combine(baseDir.FullName, "App.db");

            optionsBuilder.UseSqlite($"Data Source='{dataSource}'");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShellProcess>(entities =>
            {
                entities.ToTable("Consoles");

                entities.Property(e => e.Icon).IsRequired().HasMaxLength(ShellProcess.IconMaxLength);
                entities.Property(e => e.Name).IsRequired().HasMaxLength(ShellProcess.NameMaxLength);
                entities.Property(e => e.ProcessExecutablePath).IsRequired().HasMaxLength(ShellProcess.ProcessExecutablePathMaxLength);
                entities.Property(e => e.ProcessStartupArgs).HasMaxLength(ShellProcess.ProcessStartupArgsMaxLength);

                entities.HasData(DefaultConsolesSeeder.GetEntities());
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
