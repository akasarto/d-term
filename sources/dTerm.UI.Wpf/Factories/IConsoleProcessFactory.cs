using dTerm.Core.Processes;
using System.Diagnostics;

namespace dTerm.UI.Wpf.Factories
{
	public interface IConsoleProcessFactory
	{
		IConsoleProcess CreateProcess(ProcessStartInfo processStartInfo, int timeoutSeconds = 5);
	}
}
