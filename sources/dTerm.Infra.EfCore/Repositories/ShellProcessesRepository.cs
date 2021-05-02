using dTerm.Core;
using dTerm.Core.Reposistories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dTerm.Infra.EfCore.Repositories
{
    public class ShellProcessesRepository : IShellProcessesRepository
    {
        public async Task<ProcessEntity> CreateAsync(ProcessEntity shellProcess)
        {
            if (shellProcess.Id == Guid.Empty)
            {
                shellProcess.Id = Guid.NewGuid();
            }

            using (var context = new AppDbContext())
            {
                var newEntry = await context.Consoles.AddAsync(shellProcess);

                _ = context.SaveChanges();

                return newEntry.Entity;
            }
        }

        public async Task<List<ProcessEntity>> ReadAllAsync()
        {
            using (var context = new AppDbContext())
            {
                var entities = await context.Consoles.ToListAsync();

                return entities;
            }
        }

        public async Task<ProcessEntity> ReadByIdAsync(Guid shellProcessId)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Consoles.SingleOrDefaultAsync(e => e.Id.Equals(shellProcessId));

                return entity;
            }
        }

        public async Task<ProcessEntity> UpdateAsync(ProcessEntity shellProcess)
        {
            using (var context = new AppDbContext())
            {
                context.Consoles.Update(shellProcess);

                _ = await context.SaveChangesAsync();

                return shellProcess;
            }
        }

        public async Task DeleteAsync(Guid shellProcessId)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Consoles.SingleOrDefaultAsync(e => e.Id.Equals(shellProcessId));

                context.Consoles.Remove(entity);

                _ = await context.SaveChangesAsync();
            }
        }
    }
}
