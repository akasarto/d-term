using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
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
		private IConsoleInstanceFactory _consoleInstanceFactory;
		private IConsoleProcess _selectedConsoleInstance;

		public ShellViewModel(IEnumerable<ConsoleDescriptor> consoleDescriptors, IConsoleInstanceFactory consoleInstanceFactory)
		{
			_consoleDescriptors = consoleDescriptors ?? throw new ArgumentNullException(nameof(consoleDescriptors), nameof(ShellViewModel));
			_consoleInstanceFactory = consoleInstanceFactory ?? throw new ArgumentNullException(nameof(consoleInstanceFactory), nameof(ShellViewModel));

			//
			ConsoleInstances = new ObservableCollection<IConsoleProcess>();

			CreateConsoleProcessInstanceCommand = new RelayCommand<ConsoleDescriptor>(
				CreateConsoleProcessInstance,
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

		public IConsoleProcess SelectedConsoleInstance
		{
			get => _selectedConsoleInstance;
			set => Set(ref _selectedConsoleInstance, value);
		}

		public void OnViewClosing()
		{
			foreach (var instance in ConsoleInstances)
			{
				//instance.ProcessStatusChanged;
				instance.Terminate();
			}
		}

		private void CreateConsoleProcessInstance(ConsoleDescriptor descriptor)
		{
			var consoleInstance = _consoleInstanceFactory.CreateInstance(descriptor);

			consoleInstance.ProcessStatusChanged += (sender, args) =>
			{
				var instance = sender as IConsoleProcess;

				if (instance == null)
				{
					throw new Exception("Invalid console instance event parameters", new ArgumentNullException(nameof(sender), nameof(CreateConsoleProcessInstance)));
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
			};

			consoleInstance.Initialize((process) =>
			{
				User32Methods.ShowWindow(process.MainWindowHandle, ShowWindowCommands.SW_HIDE);
			});
		}

		private void OnConsoleInstanceInitialized(IConsoleProcess consoleInstance, ProcessEventArgs args)
		{
			ConsoleInstances.Add(consoleInstance);
		}

		private void OnConsoleInstanceTerminated(IConsoleProcess consoleInstance, ProcessEventArgs args)
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
