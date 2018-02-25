using AutoMapper;
using Splat;
using System.Windows;
using System.Windows.Threading;
using UI.Wpf.Mappings;

namespace UI.Wpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public App()
		{
			AppBootstrapper.Initialize();

			var container = Locator.CurrentMutable;

			Mapper.Initialize(config =>
			{
				config.AddProfile(container.GetService<MapperProfileConsoles>());
				config.AddProfile(container.GetService<MapperProfileNotebooks>());
			});

			// App startup
			Startup += (object sender, StartupEventArgs args) =>
			{
				MainWindow = container.GetService<ShellView>();
				MainWindow.DataContext = container.GetService<IShellViewModel>();
				MainWindow.Show();
			};

			// App exceptions
			DispatcherUnhandledException += (object sender, DispatcherUnhandledExceptionEventArgs args) =>
			{

			};

			// App shutdown
			Exit += (object sender, ExitEventArgs args) =>
			{
				container.Dispose();
			};
		}
	}
}
