using AutoMapper;
using Splat;
using System.Windows;
using System.Windows.Threading;
using UI.Wpf.Mappings;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public partial class App : Application
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public App()
		{
			AppBootstrap.Initialize();

			var container = Locator.CurrentMutable;

			Mapper.Initialize(config =>
			{
				config.AddProfile(container.GetService<MapperProfileConsoles>());
			});

			//
			Startup += (object sender, StartupEventArgs args) =>
			{
				var shellViewModel = container.GetService<IShellViewModel>();

				MainWindow = new ShellView(shellViewModel);

				MainWindow.Show();
			};

			//
			DispatcherUnhandledException += (object sender, DispatcherUnhandledExceptionEventArgs args) =>
			{
				// ToDo: Log exception.
			};

			//
			Exit += (object sender, ExitEventArgs args) =>
			{
				container.Dispose();
			};
		}
	}
}
