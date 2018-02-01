using dTerm.Consoles.Core;
using dTerm.Consoles.Processes;
using SimpleInjector;
using dTerm.UI.Wpf.Shell;
using dTerm.UI.Wpf.Consoles;

namespace dTerm.UI.Wpf
{
	public class StartupContainer : Container
	{
		public StartupContainer()
		{
			Register<ShellView>();
			Register<ShellViewModel>();

			Register<ConsoleInstanceView>();
			Register<ConsoleInstanceViewModel>();

			Register<IConsoleProcessService, ConsoleProcessService>(Lifestyle.Singleton);
		}
	}
}
