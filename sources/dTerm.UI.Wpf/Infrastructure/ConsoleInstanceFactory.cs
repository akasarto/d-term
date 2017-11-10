using dTerm.Core;
using dTerm.UI.Wpf.Models;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ConsoleInstanceFactory : IConsoleInstanceFactory
	{
		public IConsoleInstance CreateInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return null;
			}

			return new ConsoleInstance(descriptor.ProcessStartInfo, descriptor.DefautStartupTimeoutSeconds);
		}
	}
}
