using dTerm.Infra.EfCore;
using dTerm.UI.Wpf.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace dTerm.UI.Wpf
{
    public partial class App
    {
        public static void ConfigureAndRegisterDependencies(IServiceCollection services)
        {
            // Data Persistence
            services.AddDbContext<AppDbContext>(ServiceLifetime.Transient);
            services.AddTransient<AppDbContextFactory>();

            // Windows
            services.AddTransient<MainWindow>();
            services.AddTransient<SettingsWindow>();
            services.AddTransient<WindowFactory>();
        }
    }
}
