using dTerm.Core.Processes;
using dTerm.UI.Wpf.Models;

namespace dTerm.UI.Wpf.Infrastructure
{
	public interface IConsoleInstanceFactory
	{
		IConsoleInstance CreateInstance(ConsoleDescriptor descriptor);
	}
}
