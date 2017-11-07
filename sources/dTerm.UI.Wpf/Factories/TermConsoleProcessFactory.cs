using System.Diagnostics;
using dTerm.Core.Processes;

namespace dTerm.UI.Wpf.Factories
{
	public class TermConsoleProcessFactory : IConsoleProcessFactory
	{
		public IConsoleProcess CreateProcess(ProcessStartInfo processStartInfo, int timeoutSeconds = 5)
		{
			if (processStartInfo == null || !processStartInfo.PathExists())
			{
				return null;
			}

			return new TermConsoleProcess(processStartInfo);
		}
	}
}
