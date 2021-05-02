using dTerm.Core.Reposistories;
using dTerm.Infra.EfCore;
using dTerm.Infra.EfCore.Repositories;
using dTerm.UI.Wpf.Views;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Splat;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class App : Application
    {
        public App()
        {
            var defaultCultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = defaultCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCultureInfo;

            AppDomain.CurrentDomain.UnhandledException += AppGlobalExceptionsHandler;

            RegisterDependencyInjectionTypes();
        }

        private void RegisterDependencyInjectionTypes()
        {
            // Views
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

            // View Models
            Locator.CurrentMutable.RegisterLazySingleton(() => new MainWindowViewModel());

            // Repositories
            Locator.CurrentMutable.Register<IShellProcessesRepository>(() => new ShellProcessesRepository());
        }

        private void AppGlobalExceptionsHandler(object sender, UnhandledExceptionEventArgs eventArgs)
        {
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            InitData();
        }

        private static void InitData()
        {
            using var context = new AppDbContext();

            context.Database.Migrate();
        }
    }
}
