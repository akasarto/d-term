using dTerm.Core;
using dTerm.UI.Wpf.Models;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ConsoleInstanceFactory : IConsoleInstanceFactory
	{
		public IConsoleProcess CreateInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return null;
			}

			return new ConsoleProcess(descriptor.ProcessStartInfo, descriptor.DefautStartupTimeoutSeconds)
			{
				Name = descriptor.ConsoleName,
				Type = descriptor.ConsoleType
			};
		}
	}
}
