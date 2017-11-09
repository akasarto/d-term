using dTerm.Core.Processes;
using dTerm.UI.Wpf.Models;
using System;

namespace dTerm.UI.Wpf.Infrastructure
{
	public interface IConsoleInstanceFactory
	{
		IConsoleInstance CreateInstance(IntPtr ownerHandle, ConsoleDescriptor descriptor);
	}
}
