using dTerm.Core;
using dTerm.UI.Wpf.Factories;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ShellViewModel : ObservableObject
	{
		private IntPtr _shellViewHandle;
		private IConsoleProcessFactory _consoleProcessFactory;
		private ConsoleInstance _selectedConsoleInstance;

		public ShellViewModel(IConsoleProcessFactory consoleProcessFactory, IEnumerable<ConsoleOption> consoleOptions)
		{
			_consoleProcessFactory = consoleProcessFactory ?? throw new ArgumentNullException(nameof(consoleProcessFactory), nameof(ShellViewModel));

			//
			ConsoleOptions = consoleOptions ?? throw new ArgumentNullException(nameof(consoleOptions), nameof(ShellViewModel));
			ConsoleInstances = new ObservableCollection<ConsoleInstance>();

			//
			CreateConsoleProcessInstanceCommand = new RelayCommand<ConsoleOption>(
				CreateConsoleProcessInstance,
				CanCreateConsoleProcessInstance
			);
		}

		public IntPtr ShellViewHandle
		{
			get => _shellViewHandle;
			set => Set(ref _shellViewHandle, value);
		}

		public IEnumerable<ConsoleOption> ConsoleOptions { get; private set; }

		public ObservableCollection<ConsoleInstance> ConsoleInstances { get; private set; }

		public RelayCommand<ConsoleOption> CreateConsoleProcessInstanceCommand { get; private set; }

		public ConsoleInstance SelectedConsoleInstance
		{
			get => _selectedConsoleInstance;
			set => Set(ref _selectedConsoleInstance, value);
		}

		private void CreateConsoleProcessInstance(ConsoleOption consoleOption)
		{
			var consoleProcess = _consoleProcessFactory.CreateProcess(consoleOption.ProcessStartInfo);

			if (consoleProcess.Start())
			{
				Win32Api.TakeOwnership(
					consoleProcess.MainWindowHandle,
					ShellViewHandle
				);

				var consoleInstance = new ConsoleInstance(
					consoleOption.Description,
					consoleOption.ConsoleType,
					consoleProcess
				);

				ConsoleInstances.Add(consoleInstance);
			}
		}

		private bool CanCreateConsoleProcessInstance(ConsoleOption consoleOption)
		{
			if (consoleOption == null || consoleOption.ProcessStartInfo == null)
			{
				return true;
			}

			return consoleOption.IsSupported;
		}
	}
}
