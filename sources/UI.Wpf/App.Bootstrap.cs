using Processes.Core;
using Processes.Data.LiteDB;
using Processes.SystemDiagnostics;
using FluentValidation;
using Notebook.Core;
using Notebook.Data.LiteDB;
using ReactiveUI;
using Splat;
using System.Reflection;
using UI.Wpf.Processes;
using UI.Wpf.Mappings;
using UI.Wpf.Settings;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	/// <summary>
	/// Main app container bootstrapper.
	/// </summary>
	public static class AppBootstrap
	{
		//
		private static IMutableDependencyResolver _container;

		/// <summary>
		/// Constructor method.
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
			_container.Register(() => new MapperProfileProcesses());
			_container.Register(() => new MapperProfileNotebooks());

			//
			_container.Register<IProcessTracker>(() => new ProcessTracker());
			_container.Register<IProcessPathBuilder>(() => new ProcessPathBuilder());

			//
			_container.Register<IProcessesRepository>(() => new ProcessesRepository(dbConnectionString));
			_container.Register<INotebooksRepository>(() => new NotebooksRepository(dbConnectionString));

			//
			_container.Register<IProcessViewModel>(() => new ProcessViewModel());
			_container.Register<IProcessesManagerViewModel>(() => new ProcessesManagerViewModel());
			_container.Register<IValidator<IProcessViewModel>>(() => new ProcessViewModelValidator());
			_container.Register<IProcessesPanelViewModel>(() => new ProcessesPanelViewModel());
			_container.Register<IProcessFactory>(() => new ProcessFactory());

			//
			_container.Register<ISettingsViewModel>(() => new SettingsViewModel());

			//
			_container.Register<IShellViewModel>(() => new ShellViewModel());

			//
			_container.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
		}
	}
}
