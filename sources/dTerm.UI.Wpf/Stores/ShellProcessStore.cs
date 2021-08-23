using dTerm.Core;
using dTerm.Infra.EfCore;
using DynamicData;
using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Stores
{
    public class ShellProcessStore
    {
        private static SourceList<ShellProcess> _items = new();

        public ReadOnlyObservableCollection<ShellProcess> CreateCollection()
        {
            var observableItems = _items.Connect();

            ReadOnlyObservableCollection<ShellProcess> _collection;

            _ = observableItems
                //.Transform(item => ShellsToolbarOptionButton.Create(item))
                .Sort(SortExpressionComparer<ShellProcess>.Ascending(item => item.OrderIndex))
                .Bind(out _collection)
                .DisposeMany()
                .Subscribe()
            ;

            return _collection;
        }

        public async Task LoadButtonOptions()
        {
            using (var context = new AppDbContext())
            {
                var entities = await context.Consoles.ToListAsync();

                _items.Clear();

                _items.AddRange(entities);
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

                _items.Add(result.Entity);
            }
        }

        public async Task Delete(Guid shellOptionId)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Consoles.SingleOrDefaultAsync(e => e.Id.Equals(shellOptionId));

                context.Consoles.Remove(entity);

                _ = await context.SaveChangesAsync();
            }
        }

        public async Task Update(ShellProcess entity)
        {
            using (var context = new AppDbContext())
            {
                context.Consoles.Update(entity);

                _ = await context.SaveChangesAsync();
            }
        }
    }
}
