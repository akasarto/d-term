using Consoles.Core;
using Consoles.Processes;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UI.Wpf.Mappings;
using UI.Wpf.Workspace;

namespace UI.Wpf
{
	public static class AppBootstrapper
	{
		public static void Initialize()
		{
			var container = Locator.CurrentMutable;

			container.InitializeSplat();
			container.InitializeReactiveUI();

			container.Register(() => new ShellView());
			container.RegisterConstant<IShellScreen>(new ShellScreen());
			container.Register<IShellViewModel>(() => new ShellViewModel());

			container.Register<IProcessTracker>(() => new ProcessTracker());
			container.Register<IProcessPathBuilder>(() => new ProcessPathBuilder());

			container.Register<IConsoleProcessService>(() => new ConsoleProcessService());

			container.Register<IWorkspaceViewModel>(() => new WorkspaceViewModel());

			container.Register(() => new MapperProfileConsoles());
			container.Register(() => new MapperProfileNotebooks());

			container.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());


			////
			//Register<ShellView>();
			//Register<IShellScreen, ShellScreen>();
			//Register<IShellViewModel, ShellViewModel>();
			//Locator.CurrentMutable.Register<IScreen>(() => GetInstance<IShellScreen>());

			//Register<WorkspaceView>();
			//Register<IWorkspaceViewModel, WorkspaceViewModel>();
			//Locator.CurrentMutable.Register<IViewFor<IWorkspaceViewModel>>(() => GetInstance<WorkspaceView>());

			//Register<SettingsView>();
			//Register<ISettingsViewModel, SettingsViewModel>();

			//Locator.CurrentMutable.Register(() => GetInstance<ISettingsViewModel>());
			//Locator.CurrentMutable.Register<IViewFor<ISettingsViewModel>>(() => GetInstance<SettingsView>());

			////
			//Register<IConsoleSettingsViewModel, ConsoleSettingsViewModel>();
			//Register<IConsoleOptionsPanelViewModel, ConsoleOptionsPanelViewModel>();

			////
			//string liteDbConnectionString = @"dTerm.db";
			//Register<IConsoleOptionsRepository>(() => new ConsoleOptionsRepository(liteDbConnectionString));
			//Register<INotebookRepository>(() => new NotebookRepository(liteDbConnectionString));

			////
			//RegisterSingleton<IProcessTracker, ProcessTracker>();
			//RegisterSingleton<IProcessPathBuilder, ProcessPathBuilder>();

			////
			//Register<IConsoleProcessService, ConsoleProcessService>();
		}
	}
}
