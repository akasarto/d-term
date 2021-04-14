using dTerm.Core;
using dTerm.Infra.EfCore;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Services
{
    public class DataServices
    {
        private readonly ObservableCollectionExtended<ProcessEntity> _shellProcessessSource;

        public DataServices()
        {
            _shellProcessessSource = new ObservableCollectionExtended<ProcessEntity>();
        }

        public async Task GetShellProcessesAsync()
        {
            using (var dbContext = new AppDbContext())
            {
                _shellProcessessSource.Clear();

                foreach (var entity in await dbContext.Consoles.ToListAsync())
                {
                    _shellProcessessSource.Add(entity);
                }
            }
        }
    }
}
