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
			ConsoleInstances = new ObservableCollection<IConsoleInstance>();

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

		public ObservableCollection<IConsoleInstance> ConsoleInstances { get; private set; }

		public void OnViewClosing()
		{
			foreach (var instance in ConsoleInstances)
			{
				instance.ProcessStatusChanged -= OnConsoleProcessStatusChanged;
				instance.Terminate();
			}
		}

		private void CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			var consoleProcess = _consoleService.CreateConsoleInstance(descriptor);

			consoleProcess.ProcessStatusChanged += OnConsoleProcessStatusChanged;

			consoleProcess.Initialize((systemProcess) =>
			{
				//ToDo: Find a better way to avoid process window flickering.
				//User32Methods.ShowWindow(systemProcess.MainWindowHandle, ShowWindowCommands.SW_HIDE);
			});
		}

		private void OnConsoleProcessStatusChanged(object sender, ProcessEventArgs args)
		{
			var instance = sender as IConsoleInstance;

			if (instance == null)
			{
				throw new Exception("Invalid console process event parameters", new ArgumentNullException(nameof(sender), nameof(CreateConsoleInstance)));
			}

			switch (args.Status)
			{
				case ProcessStatus.Initialized:
					OnConsoleInstanceInitialized(instance, args);
					break;
				case ProcessStatus.Terminated:
					OnConsoleInstanceTerminated(instance, args);
					break;
			}
		}

		private void OnConsoleInstanceInitialized(IConsoleInstance consoleInstance, ProcessEventArgs args)
		{
			ConsoleInstances.Add(consoleInstance);
		}

		private void OnConsoleInstanceTerminated(IConsoleInstance consoleInstance, ProcessEventArgs args)
		{
			UIService.Invoke(() =>
			{
				ConsoleInstances.Remove(consoleInstance);
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
