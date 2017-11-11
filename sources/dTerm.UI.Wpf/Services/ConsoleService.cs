using System;
using dTerm.Core;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using System.Windows;
using System.Windows.Interop;
using dTerm.UI.Wpf.Views;

namespace dTerm.UI.Wpf.Services
{
	public class ConsoleService : IConsoleService
	{
		public IConsoleInstance CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return null;
			}

			return new ConsoleInstance(descriptor.ConsoleType, descriptor.ProcessStartInfo, descriptor.DefautStartupTimeoutSeconds)
			{
				Name = descriptor.ConsoleName
			};
		}

		public ConsoleViewModel CreateConsoleViewModel(IConsoleInstance consoleInstance)
		{
			return new ConsoleViewModel(consoleInstance);
		}

		public void CreateConsoleView(IntPtr ownerHandle, ConsoleViewModel viewModel)
		{
			var ownerView = HwndSource.FromHwnd(ownerHandle).RootVisual as Window;

			var consoleView = new ConsoleView()
			{
				Owner = ownerView ?? throw new InvalidOperationException(nameof(CreateConsoleView), new ArgumentException(nameof(ConsoleService), nameof(ownerHandle))),
				DataContext = viewModel ?? throw new InvalidOperationException(nameof(CreateConsoleView), new ArgumentNullException(nameof(viewModel), nameof(ConsoleService)))
			};

			consoleView.Show();
		}
	}
}
