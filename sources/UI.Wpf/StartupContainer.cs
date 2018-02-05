using AutoMapper;
using Consoles.Core;
using Consoles.Data.SQLite;
using Consoles.Processes;
using Notebook.Core;
using Notebook.Data.SQLite;
using SimpleInjector;
using UI.Wpf.Consoles;
using UI.Wpf.MapperProfiles;
using UI.Wpf.Shell;

namespace UI.Wpf
{
	public class StartupContainer : Container
	{
		public StartupContainer()
		{
			Register<ShellView>();
			Register<ShellViewModel>();

			Register<ConsoleInstanceView>();
			Register<ConsoleInstanceViewModel>();

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
