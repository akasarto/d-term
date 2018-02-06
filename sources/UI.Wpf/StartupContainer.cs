using AutoMapper;
using Consoles.Core;
using Consoles.Data.LiteDB;
using Consoles.Processes;
using Notebook.Core;
using Notebook.Data.LiteDB;
using SimpleInjector;
using UI.Wpf.Consoles;
using UI.Wpf.MapperProfiles;
using UI.Wpf.Notebook;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public class StartupContainer : Container
	{
		public StartupContainer()
		{
			Register<ShellView>();
			Register<ShellViewModel>();

			Register<ConsoleAreaView>();
			Register<ConsoleAreaViewModel>();

			Register<ConsoleInstanceView>();
			Register<ConsoleInstanceViewModel>();

			Register<NotebookAreaView>();
			Register<NotebookAreaViewModel>();

			Register<IConsolesRepository, ConsolesRepository>();
			Register<INotebookRepository, NotebookRepository>();

			Register<IConsoleProcessService, ConsoleProcessService>(Lifestyle.Singleton);

			Register<IMapper>(() =>
			{
				var mapConfig = new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<DefaultMapProfile>();
				});

				return new Mapper(mapConfig);
			});
		}
	}
}
