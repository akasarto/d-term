using dTerm.Domain;
using dTerm.Infra.EfCore;
using DynamicData;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Stores
{
    public class ShellsStore
    {
        private static SourceCache<ShellProcess, Guid> _items = new(item => item.Id);

        public ISourceCache<ShellProcess, Guid> GetSharedItems() => _items;

        public async Task LoadButtonOptions()
        {
            using (var context = new AppDbContext())
            {
                var entities = await context.Consoles.ToListAsync();

                _items.Clear();

                _items.AddOrUpdate(entities);
            }
        }

        public async Task<ShellProcess> GetBytId(Guid shellId)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Consoles.SingleOrDefaultAsync(e => e.Id.Equals(shellId));

                return entity;
            }
        }

        public async Task Create(string shellExePath)
        {
            var entry = new ShellProcess()
            {
                Id = Guid.NewGuid(),
                Icon = PackIconKind.CubeOutline.ToString(),
                Name = Path.GetFileNameWithoutExtension(shellExePath),
                ProcessExecutablePath = shellExePath,
                ProcessStartupArgs = string.Empty,
                UTCCreation = DateTime.UtcNow,
                OrderIndex = int.MaxValue,
            };

            using (var context = new AppDbContext())
            {
                var result = await context.Consoles.AddAsync(entry);

                _ = context.SaveChanges();

                _items.AddOrUpdate(result.Entity);
            }
        }

        public async Task Delete(Guid shellOptionId)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Consoles.SingleOrDefaultAsync(e => e.Id.Equals(shellOptionId));

                context.Consoles.Remove(entity);

                _ = await context.SaveChangesAsync();

                _items.Remove(shellOptionId);
            }
        }

        public async Task Update(ShellProcess entity)
        {
            using (var context = new AppDbContext())
            {
                context.Consoles.Update(entity);

                _ = await context.SaveChangesAsync();

                _items.AddOrUpdate(entity);
            }
        }
    }
}
