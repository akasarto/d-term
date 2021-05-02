using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dTerm.Core.Reposistories
{
    public interface IShellProcessesRepository
    {
        Task<ProcessEntity> CreateAsync(ProcessEntity shellProcess);
        Task<List<ProcessEntity>> ReadAllAsync();
        Task<ProcessEntity> ReadByIdAsync(Guid shellProcessId);
        Task<ProcessEntity> UpdateAsync(ProcessEntity shellProcess);
        Task DeleteAsync(Guid shellProcessId);
    }
}
