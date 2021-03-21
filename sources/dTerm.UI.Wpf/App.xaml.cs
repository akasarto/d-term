using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app.json")
                .AddEnvironmentVariables()
                .AddUserSecrets("dTerm")
                .AddCommandLine(Environment.GetCommandLineArgs())
                .Build()
            ;

            AppDomain.CurrentDomain.UnhandledException += AppGlobalExceptionsHandler;

            Current.Startup += AppStartup;
        }

        private void AppGlobalExceptionsHandler(object sender, UnhandledExceptionEventArgs eventArgs)
        {
        }

        private void AppStartup(object sender, StartupEventArgs eventArgs)
        {
            var services = new ServiceCollection();

            BuildContainer.RegisterServiceTypes(services);

            ServiceProvider = services.BuildServiceProvider(validateScopes: true);
        }
    }
}
