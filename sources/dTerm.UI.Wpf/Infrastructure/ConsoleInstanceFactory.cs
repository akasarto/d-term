using dTerm.Core.Processes;
using dTerm.UI.Wpf.Models;
using System;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ConsoleInstanceFactory : IConsoleInstanceFactory
	{
		public IConsoleInstance CreateInstance(IntPtr ownerHandle, ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return null;
			}

			return new ConsoleInstance(ownerHandle, descriptor.ProcessStartInfo, descriptor.DefautStartupTimeoutSeconds)
			{
				Name = descriptor.ConsoleName,
				Type = descriptor.ConsoleType
			};
		}
	}
}
