using Consoles.Core;
using Consoles.Data.LiteDB;
using Notebook.Core;
using Notebook.Data.LiteDB;
using SimpleInjector;
using UI.Wpf.Consoles;
using UI.Wpf.Shell;

namespace UI.Wpf.Infrastructure
{
	public class CompositionRoot : Container
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public CompositionRoot()
		{
			//
			Register<ShellView>();
			Register<IShellViewModel, ShellViewModel>();

			//
			Register<IConsoleOptionsPanelViewModel, ConsoleOptionsPanelViewModel>();

			//
			string liteDbConnectionString = @"dTerm.db";
			Register<IConsoleOptionsRepository>(() => new ConsoleOptionsRepository(liteDbConnectionString));
			Register<INotebookRepository>(() => new NotebookRepository(liteDbConnectionString));
		}
	}
}
