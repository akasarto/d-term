using AutoMapper;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using UI.Wpf.Mappings;
using UI.Wpf.Processes;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public partial class App : Application
	{
		/// <summary>
		/// Static constructor.
		/// </summary>
		static App()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (object arSender, ResolveEventArgs arArgs) =>
			{
				var assembly = Assembly.GetExecutingAssembly();
				var assemblyName = new AssemblyName(arArgs.Name);

				var assemblyFullName = $"{assemblyName.Name}.dll";

				if (!assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture))
				{
					assemblyFullName = $@"{assemblyName.CultureInfo}\{assemblyFullName}";
				}

				using (var stream = assembly.GetManifestResourceStream(assemblyFullName))
				{
					if (stream == null) return null;
					var buffer = new byte[stream.Length];
					stream.Read(buffer, 0, buffer.Length);
					return Assembly.Load(buffer);
				}
			};
		}

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
			Startup += (object sender, StartupEventArgs startupEventArgs) =>
			{
				var shellView = container.GetService<IViewFor<IShellViewModel>>();

				MainWindow = shellView as Window;

				MainWindow.Events().Loaded.Subscribe(loadedArgs =>
				{
					shellView.ViewModel = container.GetService<IShellViewModel>();
				});

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
				Locator.CurrentMutable.GetService<IConsolesInteropAgent>()?.Dispose();
				Locator.CurrentMutable.GetService<IProcessesTracker>()?.Dispose();
				container.Dispose();
			};
		}
	}
}
