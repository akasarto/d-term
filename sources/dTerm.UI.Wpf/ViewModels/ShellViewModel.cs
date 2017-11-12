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

			CreateConsoleInstanceCommand = new RelayCommand<ConsoleDescriptor>(
				CreateConsoleInstance,
				CanCreateConsoleProcessInstance
			);

			HighlightConsoleInstanceCommand = new RelayCommand<IConsoleInstance>(HighlightConsoleInstance);
			TerminateConsoleInstanceCommand = new RelayCommand<IConsoleInstance>(TerminateConsoleInstance);
		}

		public IntPtr ShellViewHandle
		{
			get => _shellViewHandle;
			set => Set(ref _shellViewHandle, value);
		}

		public IEnumerable<ConsoleDescriptor> ConsoleDescriptors => _consoleDescriptors;

		public RelayCommand<ConsoleDescriptor> CreateConsoleInstanceCommand { get; private set; }
		public RelayCommand<IConsoleInstance> HighlightConsoleInstanceCommand { get; private set; }
		public RelayCommand<IConsoleInstance> TerminateConsoleInstanceCommand { get; private set; }

		public ObservableCollection<IConsoleInstance> ConsoleInstances { get; private set; }

		public void OnViewClosing()
		{
			foreach (var instance in ConsoleInstances)
			{
				instance.ProcessTerminated -= OnConsoleProcessTerminated;
				instance.Terminate();
			}
		}

		private void CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			var consoleInstance = _consoleService.CreateConsoleInstance(descriptor);

			consoleInstance.ProcessTerminated += OnConsoleProcessTerminated;

			if (!consoleInstance.Initialize())
			{
				//TODO: Notify process failure to start.
				return;
			}

			ConsoleInstances.Add(consoleInstance);
		}

		private bool CanCreateConsoleProcessInstance(ConsoleDescriptor descriptor)
		{
			if (descriptor == null || descriptor.ProcessStartInfo == null)
			{
				return true;
			}

			return descriptor.ProcessCanStart;
		}

		private void HighlightConsoleInstance(IConsoleInstance consoleInstance)
		{
			System.Windows.MessageBox.Show(consoleInstance.Name, "Hightlight");
		}

		private void TerminateConsoleInstance(IConsoleInstance consoleInstance)
		{
			System.Windows.MessageBox.Show(consoleInstance.Name, "Terminate");
		}

		private void OnConsoleProcessTerminated(object sender, EventArgs args)
		{
			var instance = sender as IConsoleInstance;

			if (instance != null)
			{
				UIService.Invoke(() => ConsoleInstances.Remove(instance));
			}
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
