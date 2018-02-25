using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using Notebook.Core;
using Notebook.Data.LiteDB;
using ReactiveUI;
using Splat;
using System.Reflection;
using UI.Wpf.Consoles;
using UI.Wpf.Mappings;
using UI.Wpf.Workspace;

namespace UI.Wpf
{
	/// <summary>
	/// Main app container bootstrapper.
	/// </summary>
	public static class AppBootstrapper
	{
		//
		private static IMutableDependencyResolver _container;

		/// <summary>
		/// Constructor method.
		/// </summary>
		static AppBootstrapper()
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
			_container.Register(() => new MapperProfileNotebooks());

			//
			_container.Register<IProcessTracker>(() => new ProcessTracker());
			_container.Register<IProcessPathBuilder>(() => new ProcessPathBuilder());

			//
			_container.Register<IConsoleOptionsRepository>(() => new ConsoleOptionsRepository(dbConnectionString));
			_container.Register<INotebooksRepository>(() => new NotebooksRepository(dbConnectionString));

			//
			_container.Register<IConsoleProcessService>(() => new ConsoleProcessService());
			_container.Register<IConsoleOptionsPanelViewModel>(() => new ConsoleOptionsPanelViewModel());
			_container.Register<IConsoleSettingsViewModel>(() => new ConsoleSettingsViewModel());

			//
			_container.Register<IWorkspaceViewModel>(() => new WorkspaceViewModel());

			//
			_container.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

			//
			_container.RegisterConstant<IShellScreen>(new ShellScreen());
			_container.Register<IShellViewModel>(() => new ShellViewModel());
			_container.Register(() => new ShellView());
		}
	}
}
