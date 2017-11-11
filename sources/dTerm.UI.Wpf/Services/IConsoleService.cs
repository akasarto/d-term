using dTerm.Core;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using System;

namespace dTerm.UI.Wpf.Services
{
	public interface IConsoleService
	{
		IConsoleInstance CreateConsoleInstance(ConsoleDescriptor consoleDescriptor);

		ConsoleViewModel CreateConsoleViewModel(IConsoleInstance consoleInstance);

		void ShowConsoleView(IntPtr ownerHandle, ConsoleViewModel consoleViewModel);

		void CloseConsoleView(ConsoleViewModel consoleViewModel);
	}
}
