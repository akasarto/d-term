using AutoMapper;
using ReactiveUI;
using SimpleInjector;
using SimpleInjector.Advanced;
using Splat;
using System.Windows;
using System.Windows.Threading;
using UI.Wpf.Mappings;
using UI.Wpf.Workspace;

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
				MainWindow = Locator.CurrentMutable.GetService<IViewFor<IWorkspaceViewModel>>() as Window;
				MainWindow.DataContext = container.GetInstance<IWorkspaceViewModel>();
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
