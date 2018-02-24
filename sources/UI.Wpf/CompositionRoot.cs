using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using Notebook.Core;
using Notebook.Data.LiteDB;
using ReactiveUI;
using SimpleInjector;
using Splat;
using UI.Wpf.Consoles;
using UI.Wpf.Workspace;

namespace UI.Wpf
{
	/// <summary>
	/// Determine all types available for dependency injection.
	/// </summary>
	public class CompositionRoot : Container
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public CompositionRoot()
		{
			//
			Register<WorkspaceView>();
			Register<IWorkspaceViewModel, WorkspaceViewModel>();
			Locator.CurrentMutable.Register<IViewFor<IWorkspaceViewModel>>(() => GetInstance<WorkspaceView>());

			Register<GeneralSettingsView>();
			Register<IGeneralSettingsViewModel, GeneralSettingsViewModel>();
			Locator.CurrentMutable.Register<IViewFor<IGeneralSettingsViewModel>>(() => GetInstance<GeneralSettingsView>());

			//
			Register<IConsoleSettingsViewModel, ConsoleSettingsViewModel>();
			Register<IConsoleOptionsPanelViewModel, ConsoleOptionsPanelViewModel>();

			//
			string liteDbConnectionString = @"dTerm.db";
			Register<IConsoleOptionsRepository>(() => new ConsoleOptionsRepository(liteDbConnectionString));
			Register<INotebookRepository>(() => new NotebookRepository(liteDbConnectionString));

			//
			RegisterSingleton<IProcessTracker, ProcessTracker>();
			RegisterSingleton<IProcessPathBuilder, ProcessPathBuilder>();

			//
			Register<IConsoleProcessService, ConsoleProcessService>();
		}
	}
}
