using FluentValidation;
using MaterialDesignThemes.Wpf;
using Processes.Core;
using Processes.Data.LiteDB;
using Processes.SystemDiagnostics;
using ReactiveUI;
using Splat;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using UI.Wpf.Mappings;
using UI.Wpf.Processes;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public static class AppBootstrap
	{
		private static IMutableDependencyResolver _container;

		/// <summary>
		/// Static constructor method.
		/// </summary>
		static AppBootstrap()
		{
			Locator.CurrentMutable.InitializeSplat();
			Locator.CurrentMutable.InitializeReactiveUI();

			_container = Locator.CurrentMutable;
		}

		/// <summary>
		/// Constructor method.
		/// </summary>
		public static void Initialize()
		{
			var dbConnectionString = @"dTerm.db";

			//
			_container.Register(() => new MapperProfileConsoles());

			//
			_container.Register<IProcessesTracker>(() => new ProcessesTracker());
			_container.Register<IProcessPathBuilder>(() => new ProcessPathBuilder());
			_container.Register<IProcessRepository>(() => new ProcessRepository(dbConnectionString));

			//
			_container.Register<IProcessFactory>(() => new ProcessFactory());
			_container.Register<IConfigsViewModel>(() => new ConfigsViewModel());
			_container.Register<IProcessViewModel>(() => new ProcessViewModel());
			_container.Register<IProcessesController>(() => new ProcessesController());
			_container.Register<IConsolesTabViewModel>(() => new ConsolesTabViewModel());
			_container.Register<IConsolesPanelViewModel>(() => new ConsolesPanelViewModel());
			_container.Register<IValidator<IProcessViewModel>>(() => new ProcessViewModelValidator());
			_container.Register<IMinimizedInstancesPanelViewModel>(() => new MinimizedInstancesPanelViewModel());
			_container.Register<ITransparencyManagerPanelViewModel>(() => new TransparencyManagerPanelViewModel());

			//
			_container.Register<IShellViewModel>(() => new ShellViewModel());

			//
			_container.RegisterLazySingleton<IAppState>(() =>
			{
				var mainView = Application.Current.MainWindow;
				var interopHelper = new WindowInteropHelper(mainView);
				return new AppState(interopHelper.Handle);
			});
			_container.RegisterLazySingleton<IConsolesInteropAgent>(() => new ConsolesInteropAgent());
			_container.RegisterLazySingleton<ISnackbarMessageQueue>(() => new SnackbarMessageQueue(
				TimeSpan.FromSeconds(5))
			);

			//
			_container.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
		}
	}
}
