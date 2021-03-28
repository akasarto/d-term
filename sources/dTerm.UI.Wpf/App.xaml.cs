using dTerm.Infra.EfCore;
using dTerm.UI.Wpf.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            Current.Startup += AppStartup;

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var defaultCultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = defaultCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCultureInfo;

            AppDomain.CurrentDomain.UnhandledException += AppGlobalExceptionsHandler;

            Configuration = new ConfigurationBuilder()
                .AddCommandLine(Environment.GetCommandLineArgs())
                .AddEnvironmentVariables()
                .AddUserSecrets("dTerm")
                .Build()
            ;
        }

        public IConfiguration Configuration { get; }

        private void AppGlobalExceptionsHandler(object sender, UnhandledExceptionEventArgs eventArgs)
        {
        }

        private void AppStartup(object sender, StartupEventArgs eventArgs)
        {
            var services = new ServiceCollection();

            ConfigureAndRegisterDependencies(services);

            _serviceProvider = services.BuildServiceProvider(validateScopes: true);

            InitializeDatabaseServicesAndDefaultData(_serviceProvider);

            MainWindow = _serviceProvider.GetService<MainWindow>();

            MainWindow.Show();
        }

        private static void InitializeDatabaseServicesAndDefaultData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetService<AppDbContext>();

            context.Database.Migrate();
        }
    }
}
