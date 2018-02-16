using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using Notebook.Core;
using Notebook.Data.LiteDB;
using SimpleInjector;
using UI.Wpf.Consoles;
using UI.Wpf.Notebook;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public class StartupContainer : Container
	{
		public StartupContainer()
		{
			//
			Register<ShellView>();
			Register<ShellViewModel>();

			//
			Register<ConsolesWorkspaceViewModel>();
			Register<ConsoleOptionsListViewModel>();
			Register<ProcessInstancesListViewModel>();
			Register<ProcessInstancesArrangeViewModel>();

			//
			Register<NoteAddViewModel>();
			Register<NoteDetailsViewModel>();
			Register<NoteDetailsListViewModel>();
			Register<NotebookWorkspaceViewModel>();

			//
			string liteDbConnectionString = @"dTerm.db";
			Register<IConsolesRepository>(() => new ConsolesRepository(liteDbConnectionString));
			Register<INotebookRepository>(() => new NotebookRepository(liteDbConnectionString));

			//
			Register<IConsolesProcessService, ConsoleProcessService>();
			RegisterSingleton<IProcessPathBuilder, ProcessPathBuilder>();
			//
			RegisterSingleton<IProcessTracker, ProcessTracker>();
		}

		protected override void Dispose(bool disposing)
		{
			GetInstance<IProcessTracker>().KillAll();

			base.Dispose(disposing);
		}
	}
}
