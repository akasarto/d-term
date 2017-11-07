using dTerm.Core;
using dTerm.UI.Wpf.Factories;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ShellViewModel : ObservableObject
	{
		private IProcessFactory _processFactory;
		private ConsoleInstanceViewModel _selectedConsoleInstance;

		public ShellViewModel(IProcessFactory processFactory, IEnumerable<IConsoleOption> consoleOptions)
		{
			_processFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory), nameof(ShellViewModel));

			//
			ConsoleOptions = consoleOptions ?? throw new ArgumentNullException(nameof(consoleOptions), nameof(ShellViewModel));
			ConsoleInstances = new ObservableCollection<ConsoleInstanceViewModel>();

			//
			CreateConsoleProcessInstanceCommand = new RelayCommand<IConsoleOption>(
				CreateConsoleProcessInstance,
				CanCreateConsoleProcessInstance
			);
		}

		public IEnumerable<IConsoleOption> ConsoleOptions { get; private set; }

		public ObservableCollection<ConsoleInstanceViewModel> ConsoleInstances { get; private set; }

		public RelayCommand<IConsoleOption> CreateConsoleProcessInstanceCommand { get; private set; }

		public ConsoleInstanceViewModel SelectedConsoleInstance
		{
			get => _selectedConsoleInstance;
			set => Set(ref _selectedConsoleInstance, value);
		}

		private void CreateConsoleProcessInstance(IConsoleOption consoleOption)
		{
			var process = _processFactory.CreateProcess(consoleOption.ProcessStartInfo);

			if (process.Start())
			{
				Win32Api.TakeOwnership(
					process.MainWindowHandle,
					new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle
				);
			}
		}

		private bool CanCreateConsoleProcessInstance(IConsoleOption consoleOption)
		{
			if (consoleOption == null || consoleOption.ProcessStartInfo == null)
			{
				return true;
			}

			return consoleOption.IsSupported;
		}
	}
}
