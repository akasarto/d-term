using dTerm.Core;
using dTerm.Core.ProcessStarters;
using dTerm.UI.Wpf.Models;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ConsoleInstanceFactory : IConsoleInstanceFactory
	{
		public IConsoleInstance Create(string consoleName, ConsoleType consoleType, ConsoleProcessStartInfo consoleProcessStartInfo, int operationsTimeoutInSecconds = 3)
		{
			return new ConsoleInstance(consoleType, consoleProcessStartInfo, operationsTimeoutInSecconds)
			{
				Name = consoleName
			};
		}
	}
}
