using dTerm.Infra.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace dTerm.UI.Wpf
{
    public partial class App
    {
        public static void ConfigureAndRegisterDependencies(IServiceCollection services)
        {
            // Data Persistence
            services.AddDbContext<AppDbContext>(ServiceLifetime.Transient);

            // Windows
            services.AddSingleton<MainWindow>();
        }
    }
}
