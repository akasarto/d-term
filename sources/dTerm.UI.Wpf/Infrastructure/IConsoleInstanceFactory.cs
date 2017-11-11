using dTerm.Core;
using dTerm.UI.Wpf.Models;

namespace dTerm.UI.Wpf.Infrastructure
{
	public interface IConsoleInstanceFactory
	{
		IConsoleProcess CreateInstance(ConsoleDescriptor descriptor);
	}
}
