using dTerm.Core;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using System;

namespace dTerm.UI.Wpf.Services
{
	public interface IConsoleService
	{
		IConsoleInstance CreateConsoleInstance(ConsoleDescriptor descriptor);

		ConsoleViewModel CreateConsoleViewModel(IConsoleInstance consoleInstance);

		void ShowConsoleView(IntPtr ownerHandle, ConsoleViewModel viewModel);

		void ShutdownConsoleView(IntPtr viewHandle);
	}
}
