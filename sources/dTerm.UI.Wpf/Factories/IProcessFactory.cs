using dTerm.Core.Processes;
using System.Diagnostics;

namespace dTerm.UI.Wpf.Factories
{
	public interface IProcessFactory
	{
		ITermProcess CreateProcess(ProcessStartInfo processStartInfo, int timeoutSeconds = 5);
	}
}
