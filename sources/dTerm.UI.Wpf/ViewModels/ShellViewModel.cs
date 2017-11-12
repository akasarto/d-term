using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ShellViewModel : ObservableObject
	{
		private IntPtr _shellViewHandle;
		private IEnumerable<ConsoleDescriptor> _consoleDescriptors;
		private Dictionary<int, ConsoleViewModel> _consoleViewModels;
		private IConsoleService _consoleService;

		public ShellViewModel(IEnumerable<ConsoleDescriptor> consoleDescriptors, IConsoleService consoleService)
		{
			_consoleDescriptors = consoleDescriptors ?? throw new ArgumentNullException(nameof(consoleDescriptors), nameof(ShellViewModel));
			_consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService), nameof(ShellViewModel));
			_consoleViewModels = new Dictionary<int, ConsoleViewModel>();

			//
			ConsoleInstances = new ObservableCollection<IConsoleInstance>();
			ConsoleInstances.CollectionChanged += OnConsoleInstancesCollectionChanged;

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

		private bool CanCreateConsoleProcessInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return true;
			}

			return descriptor.IsSupported;
		}

		private void CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			var consoleProcess = _consoleService.CreateConsoleInstance(descriptor);

			consoleProcess.ProcessStatusChanged += OnConsoleProcessStatusChanged;

			consoleProcess.Initialize();
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
				case ProcessStatus.Timeout:
					//TODO: Notify process failure to start.
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

		private void OnConsoleInstancesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (IConsoleInstance consoleInstance in e.NewItems)
					{
						CreateConsoleView(consoleInstance);
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					foreach (IConsoleInstance consoleInstance in e.OldItems)
					{
						DestroyConsoleView(consoleInstance);
					}
					break;
			}
		}

		private void CreateConsoleView(IConsoleInstance consoleInstance)
		{
			var consoleViewModel = _consoleService.CreateConsoleViewModel(consoleInstance);

			_consoleViewModels.Add(consoleInstance.ProcessId, consoleViewModel);

			_consoleService.ShowConsoleView(ShellViewHandle, consoleViewModel);
		}

		private void DestroyConsoleView(IConsoleInstance consoleInstance)
		{
			var processId = consoleInstance.ProcessId;

			if (_consoleViewModels.ContainsKey(processId))
			{
				var consoleViewModel = _consoleViewModels[processId];

				_consoleService?.CloseConsoleView(consoleViewModel);
			}

			_consoleViewModels.Remove(processId);
		}
	}
}
