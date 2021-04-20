using dTerm.Core;
using dTerm.Infra.EfCore;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Services
{
    public class ShellProcessesData : ObservableCollectionExtended<ProcessEntity>
    {
        public async Task Add(string executablePath)
        {
            var entity = new ProcessEntity()
            {
                Id = Guid.NewGuid(),
                Icon = MaterialDesignThemes.Wpf.PackIconKind.CubeOutline.ToString(),
                Name = Path.GetFileNameWithoutExtension(executablePath),
                ProcessExecutablePath = executablePath,
                ProcessStartupArgs = string.Empty,
                ProcessType = ProcessType.Shell,
                UTCCreation = DateTime.UtcNow,
                OrderIndex = int.MaxValue,
            };

            using (var context = new AppDbContext())
            {
                var newEntry = await context.Consoles.AddAsync(entity);

                _ = context.SaveChanges();

                Add(newEntry.Entity);
            }
        }

        public async Task LoadASync()
        {
            using (var context = new AppDbContext())
            {
                var entities = await context.Consoles.ToListAsync();

                AddRange(entities);
            }
        }
    }
}
