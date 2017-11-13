using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using dTerm.UI.Wpf.Views;
using System;
using System.Windows;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Services
{
	public class ConsoleService : IConsoleService
	{
		private IConsoleInstanceFactory _consoleInstanceFactory;

		public ConsoleService(IConsoleInstanceFactory consoleInstanceFactory)
		{
			_consoleInstanceFactory = consoleInstanceFactory ?? throw new ArgumentNullException(nameof(consoleInstanceFactory), nameof(ConsoleService));
		}

		public IConsoleInstance CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null)
			{
				throw new InvalidOperationException(nameof(CreateConsoleInstance), new ArgumentNullException(nameof(descriptor), nameof(ConsoleService)));
			}

			return _consoleInstanceFactory.Create(descriptor.Description, descriptor.ConsoleType, descriptor.ProcessStartInfo, descriptor.OperationsTimeoutInSeconds);
		}

		public ConsoleViewModel CreateConsoleViewModel(IConsoleInstance consoleInstance)
		{
			return new ConsoleViewModel(consoleInstance);
		}

		public void ShowConsoleView(IntPtr ownerHandle, ConsoleViewModel consoleViewModel)
		{
			var ownerView = HwndSource.FromHwnd(ownerHandle).RootVisual as Window;

			if (ownerView == null)
			{
				throw new InvalidOperationException(nameof(ShowConsoleView), new ArgumentNullException(nameof(ownerView), nameof(ConsoleService)));
			}

			var consoleView = new ConsoleView()
			{
				Owner = ownerView ?? throw new InvalidOperationException(nameof(ShowConsoleView), new ArgumentException(nameof(ConsoleService), nameof(ownerHandle))),
				DataContext = consoleViewModel ?? throw new InvalidOperationException(nameof(ShowConsoleView), new ArgumentNullException(nameof(consoleViewModel), nameof(ConsoleService)))
			};

			consoleView.Show();
		}

		public void CloseConsoleView(ConsoleViewModel consoleViewModel)
		{
			if (consoleViewModel == null)
			{
				throw new InvalidOperationException(nameof(CloseConsoleView), new ArgumentNullException(nameof(consoleViewModel), nameof(ConsoleService)));
			}

			var viewHandle = consoleViewModel.ConsoleViewHandle;
			var viewInstance = HwndSource.FromHwnd(viewHandle)?.RootVisual as ConsoleView;

			viewInstance?.Owner?.Activate();

			viewInstance?.Close();
		}
	}
}
