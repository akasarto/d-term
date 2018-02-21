using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using MaterialDesignThemes.Wpf;
using Notebook.Core;
using Notebook.Data.LiteDB;
using SimpleInjector;
using System;
using UI.Wpf.Consoles;
using UI.Wpf.Infrastructure;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public class StartupContainer : Container
	{
		public StartupContainer()
		{
			//
			Register<ShellView>();
			Register<IShellViewModel, ShellViewModel>();

			//
			Register<IConsoleOptionsPanelViewModel, ConsoleOptionsPanelViewModel>();

			//
			Register<IConsoleProcessService, ConsoleProcessService>();
			RegisterSingleton<IProcessPathBuilder, ProcessPathBuilder>();

			//
			string liteDbConnectionString = @"dTerm.db";
			Register<IConsoleOptionsRepository>(() => new ConsoleOptionsRepository(liteDbConnectionString));
			Register<INotebookRepository>(() => new NotebookRepository(liteDbConnectionString));

			//
			RegisterSingleton<IProcessTracker, ProcessTracker>();
			RegisterSingleton<ISnackbarMessageQueue>(() => new SnackbarMessageQueue(TimeSpan.FromSeconds(3)));

			//
			Register<IViewModelFactory, SimpleInjectorViewModelFactory>();
		}

		protected override void Dispose(bool disposing)
		{
			GetInstance<IProcessTracker>().KillAll();

			base.Dispose(disposing);
		}
	}
}
