using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dTerm.Core.Reposistories
{
    public interface IShellProcessesRepository
    {
        Task<ShellProcess> CreateAsync(ShellProcess shellProcess);
        Task<List<ShellProcess>> ReadAllAsync();
        Task<ShellProcess> ReadByIdAsync(Guid shellProcessId);
        Task<ShellProcess> UpdateAsync(ShellProcess shellProcess);
        Task DeleteAsync(Guid shellProcessId);
    }
}
