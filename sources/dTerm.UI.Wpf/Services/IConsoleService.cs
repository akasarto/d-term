using dTerm.Core;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using System;

namespace dTerm.UI.Wpf.Services
{
	public interface IConsoleService
	{
		IConsoleProcess CreateConsoleProcess(ConsoleDescriptor descriptor);

		ConsoleInstanceViewModel CreateConsoleViewModel(IConsoleProcess consoleProcess);

		void CreateConsoleView(IntPtr ownerHandle, ConsoleInstanceViewModel viewModel);
	}
}
