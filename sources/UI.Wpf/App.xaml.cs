using ReactiveUI;
using Splat;
using System.Windows;
using System.Windows.Threading;
using UI.Wpf.Infrastructure;

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

			Locator.CurrentMutable.InitializeSplat();
			Locator.CurrentMutable.InitializeReactiveUI();

			ShutdownMode = ShutdownMode.OnMainWindowClose;

			// App startup
			Startup += (object sender, StartupEventArgs args) =>
			{
				MainWindow = new ShellView();
				MainWindow.DataContext = new ShellViewModel();
				MainWindow.Show();
			};

			// App exceptions
			DispatcherUnhandledException += (object sender, DispatcherUnhandledExceptionEventArgs args) =>
			{

			};

			// App shutdown
			Exit += (object sender, ExitEventArgs args) =>
			{

			};
		}
	}
}
