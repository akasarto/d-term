using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinApi.User32;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ShellViewModel : ObservableObject
	{
		private IntPtr _shellViewHandle;
		private IEnumerable<ConsoleDescriptor> _consoleDescriptors;
		private IConsoleService _consoleService;

		public ShellViewModel(IEnumerable<ConsoleDescriptor> consoleDescriptors, IConsoleService consoleService)
		{
			_consoleDescriptors = consoleDescriptors ?? throw new ArgumentNullException(nameof(consoleDescriptors), nameof(ShellViewModel));
			_consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService), nameof(ShellViewModel));

			//
			ConsoleInstances = new ObservableCollection<IConsoleProcess>();

			CreateConsoleProcessInstanceCommand = new RelayCommand<ConsoleDescriptor>(
				CreateConsoleInstance,
				CanCreateConsoleProcessInstance
			);
		}

		public IntPtr ShellViewHandle
		{
			get => _shellViewHandle;
			set => Set(ref _shellViewHandle, value);
		}

		public IEnumerable<ConsoleDescriptor> ConsoleDescriptors => _consoleDescriptors;

		public RelayCommand<ConsoleDescriptor> CreateConsoleProcessInstanceCommand { get; private set; }

		public ObservableCollection<IConsoleProcess> ConsoleInstances { get; private set; }

		public void OnViewClosing()
		{
			foreach (var instance in ConsoleInstances)
			{
				//instance.ProcessStatusChanged;
				instance.Terminate();
			}
		}

		private void CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			var consoleProcess = _consoleService.CreateConsoleProcess(descriptor);

			consoleProcess.ProcessStatusChanged += (sender, args) =>
			{
				var process = sender as IConsoleProcess;

				if (process == null)
				{
					throw new Exception("Invalid console process event parameters", new ArgumentNullException(nameof(sender), nameof(CreateConsoleInstance)));
				}

				switch (args.Status)
				{
					case ProcessStatus.Initialized:
						OnConsoleProcessInitialized(process, args);
						break;
					case ProcessStatus.Terminated:
						OnConsoleProcessTerminated(process, args);
						break;
				}
			};

			consoleProcess.Initialize((systemProcess) =>
			{
				//ToDo: Find a better way to avoid process window flickering.
				User32Methods.ShowWindow(systemProcess.MainWindowHandle, ShowWindowCommands.SW_HIDE);
			});
		}

		private void OnConsoleProcessInitialized(IConsoleProcess consoleProcess, ProcessEventArgs args)
		{
			var consoleViewModel = _consoleService.CreateConsoleViewModel(consoleProcess);

			ConsoleInstances.Add(consoleProcess);
		}

		private void OnConsoleProcessTerminated(IConsoleProcess consoleProcess, ProcessEventArgs args)
		{
			UIService.Invoke(() =>
			{
				ConsoleInstances.Remove(consoleProcess);
			});
		}

		private bool CanCreateConsoleProcessInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return true;
			}

			return descriptor.IsSupported;
		}
	}
}
