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
		public IConsoleInstance CreateConsoleInstance(ConsoleDescriptor consoleDescriptor)
		{
			if (consoleDescriptor == null || consoleDescriptor.ProcessStartInfo == null)
			{
				return null;
			}

			return new ConsoleInstance(consoleDescriptor.ConsoleType, consoleDescriptor.ProcessStartInfo, consoleDescriptor.DefautStartupTimeoutSeconds)
			{
				Name = consoleDescriptor.ConsoleName
			};
		}

		public ConsoleViewModel CreateConsoleViewModel(IConsoleInstance consoleInstance)
		{
			return new ConsoleViewModel(consoleInstance);
		}

		public void ShowConsoleView(IntPtr ownerHandle, ConsoleViewModel consoleViewModel)
		{
			var ownerView = HwndSource.FromHwnd(ownerHandle).RootVisual as Window;

			var consoleView = new ConsoleView()
			{
				Owner = ownerView ?? throw new InvalidOperationException(nameof(ShowConsoleView), new ArgumentException(nameof(ConsoleService), nameof(ownerHandle))),
				DataContext = consoleViewModel ?? throw new InvalidOperationException(nameof(ShowConsoleView), new ArgumentNullException(nameof(consoleViewModel), nameof(ConsoleService)))
			};

			consoleView.Show();
		}

		public void CloseConsoleView(ConsoleViewModel consoleViewModel)
		{
			if (!consoleViewModel.IsClosing)
			{
				var viewHandle = consoleViewModel.ConsoleViewHandle;
				var viewInstance = HwndSource.FromHwnd(viewHandle)?.RootVisual as ConsoleView;

				if (viewInstance == null)
				{
					return;
				}

				viewInstance.Close();
			}
		}
	}
}
