using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using Notebook.Core;
using Notebook.Data.LiteDB;
using ReactiveUI;
using SimpleInjector;
using Splat;
using System;
using System.Collections.Generic;
using System.Reflection;
using UI.Wpf.Consoles;
using UI.Wpf.Settings;
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

			Register<SettingsView>();
			Register<ISettingsViewModel, SettingsViewModel>();
			Locator.CurrentMutable.Register<IViewFor<ISettingsViewModel>>(() => GetInstance<SettingsView>());

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
