using dTerm.Core;
using dTerm.Core.ProcessStarters;

namespace dTerm.UI.Wpf.Infrastructure
{
	public interface IConsoleInstanceFactory
	{
		IConsoleInstance Create(string consoleName, ConsoleType consoleType, ConsoleProcessStartInfo consoleProcessStartInfo, int operationsTimeoutInSecconds = 3);
	}
}
