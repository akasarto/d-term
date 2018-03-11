using FluentValidation;
using Notebook.Core;
using Notebook.Data.LiteDB;
using Processes.Core;
using Processes.Data.LiteDB;
using Processes.SystemDiagnostics;
using ReactiveUI;
using Splat;
using System.Reflection;
using UI.Wpf.Consoles;
using UI.Wpf.Mappings;
using UI.Wpf.Settings;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public static class AppBootstrap
	{
		//
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
			_container.Register(() => new MapperProfileProcesses());
			_container.Register(() => new MapperProfileNotebooks());

			//
			_container.Register<IProcessTracker>(() => new ProcessTracker());
			_container.Register<IProcessPathBuilder>(() => new ProcessPathBuilder());

			//
			_container.Register<IProcessRepository>(() => new ProcessRepository(dbConnectionString));
			_container.Register<INotebooksRepository>(() => new NotebooksRepository(dbConnectionString));

			//
			_container.Register<IConsoleProcessFactory>(() => new ConsoleProcessFactory());
			_container.Register<IConsoleOptionViewModel>(() => new ConsoleOptionViewModel());
			_container.Register<IConsolesPanelViewModel>(() => new ConsolesPanelViewModel());
			_container.Register<IConsolesManagerViewModel>(() => new ConsolesManagerViewModel());
			_container.Register<IValidator<IConsoleOptionViewModel>>(() => new ConsoleOptionViewModelValidator());

			//
			_container.Register<ISettingsViewModel>(() => new SettingsViewModel());

			//
			_container.Register<IShellViewModel>(() => new ShellViewModel());

			//
			_container.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
		}
	}
}
