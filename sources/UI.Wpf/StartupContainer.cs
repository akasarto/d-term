using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using MaterialDesignThemes.Wpf;
using Notebook.Core;
using Notebook.Data.LiteDB;
using SimpleInjector;
using System;
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
			Register<ConsoleProcessInstancesListViewModel>();
			Register<ConsoleProcessInstancesArrangeViewModel>();

			//
			Register<NoteAddViewModel>();
			Register<NoteDetailsViewModel>();
			Register<NoteDetailsListViewModel>();
			Register<NotebookWorkspaceViewModel>();

			//
			string liteDbConnectionString = @"dTerm.db";
			Register<IConsoleOptionsRepository>(() => new ConsoleOptionsRepository(liteDbConnectionString));
			Register<INotebookRepository>(() => new NotebookRepository(liteDbConnectionString));

			//
			Register<IConsoleProcessService, ConsoleProcessService>();
			RegisterSingleton<IProcessPathBuilder, ProcessPathBuilder>();

			//
			RegisterSingleton<IProcessTracker, ProcessTracker>();
			RegisterSingleton<ISnackbarMessageQueue>(() => new SnackbarMessageQueue(TimeSpan.FromSeconds(3)));
		}

		protected override void Dispose(bool disposing)
		{
			GetInstance<IProcessTracker>().KillAll();

			base.Dispose(disposing);
		}
	}
}
