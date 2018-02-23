using AutoMapper;
using ReactiveUI;
using Splat;
using System.Windows;
using System.Windows.Threading;
using UI.Wpf.Infrastructure;
using UI.Wpf.Shell;

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
			var container = new CompositionRoot();

			Mapper.Initialize(config =>
			{
				config.AddProfile(container.GetInstance<MapperProfileConsoles>());
				config.AddProfile(container.GetInstance<MapperProfileNotebooks>());
			});

			Locator.CurrentMutable.InitializeSplat();
			Locator.CurrentMutable.InitializeReactiveUI();

			ShutdownMode = ShutdownMode.OnMainWindowClose;

			// App startup
			Startup += (object sender, StartupEventArgs args) =>
			{
				MainWindow = container.GetInstance<ShellView>();
				MainWindow.DataContext = container.GetInstance<IShellViewModel>();
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
