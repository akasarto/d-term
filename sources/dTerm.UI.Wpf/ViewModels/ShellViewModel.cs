using dTerm.Core;
using dTerm.Core.Events;
using dTerm.Core.ProcessTaskKillers;
using dTerm.UI.Wpf.EventMessages;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ShellViewModel : ObservableObject
	{
		private IntPtr _shellViewHandle;
		private IConsoleService _consoleService;
		private IEnumerable<ConsoleDescriptor> _consoleDescriptors;
		private Dictionary<int, ConsoleViewModel> _consoleViewModels;

		public ShellViewModel(IEnumerable<ConsoleDescriptor> consoleDescriptors, IConsoleService consoleService)
		{
			_consoleDescriptors = consoleDescriptors ?? throw new ArgumentNullException(nameof(consoleDescriptors), nameof(ShellViewModel));
			_consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService), nameof(ShellViewModel));
			_consoleViewModels = new Dictionary<int, ConsoleViewModel>();

			//
			SubscribeToEventMessages();

			//
			HiddenConsoleInstances = new ObservableCollection<IConsoleInstance>();

			//
			CreateConsoleInstanceCommand = new RelayCommand<ConsoleDescriptor>(
				CreateConsoleInstance,
				CanCreateConsoleProcessInstance
			);

			HighlightConsoleInstanceCommand = new RelayCommand<IConsoleInstance>(HighlightConsoleInstance);
			TerminateConsoleInstanceCommand = new RelayCommand<IConsoleInstance>(TerminateConsoleInstance);
		}

		public IEnumerable<ConsoleDescriptor> ConsoleDescriptors => _consoleDescriptors;
		public ObservableCollection<IConsoleInstance> HiddenConsoleInstances { get; private set; }

		public RelayCommand<ConsoleDescriptor> CreateConsoleInstanceCommand { get; private set; }
		public RelayCommand<IConsoleInstance> HighlightConsoleInstanceCommand { get; private set; }
		public RelayCommand<IConsoleInstance> TerminateConsoleInstanceCommand { get; private set; }

		public IntPtr ViewHandle => _shellViewHandle;

		public void OnViewLoaded(object sender, EventArgs args)
		{
			var interopHelper = new WindowInteropHelper(sender as Window);

			_shellViewHandle = interopHelper.Handle;
		}

		public void OnViewClosing()
		{
			var consoleTaskKiller = ConsoleTaskKiller.Create();

			foreach (var consoleViewModel in _consoleViewModels.Values)
			{
				consoleViewModel.ConsoleInstance.ProcessTerminated -= OnConsoleInstanceProcessTerminated;
				consoleTaskKiller.AddProcessId(consoleViewModel.ConsoleInstance.ProcessId);
			}

			consoleTaskKiller.Execute(throwOnEmpty: false);
		}

		private void SubscribeToEventMessages()
		{
			EventBus.Subscribe<HideConsoleMessage>((message) =>
			{
				HiddenConsoleInstances.Add(message.ConsoleInstance);
			});

			EventBus.Subscribe<ShowConsoleMessage>((message) =>
			{
				HiddenConsoleInstances.Remove(message.ConsoleInstance);
			});
		}

		private void CreateConsoleInstance(ConsoleDescriptor descriptor)
		{
			var consoleInstance = _consoleService.CreateConsoleInstance(descriptor);

			consoleInstance.ProcessTerminated += OnConsoleInstanceProcessTerminated;

			//TODO: Review
			var processInitThread = new Thread(new ThreadStart(() =>
			{
				if (!consoleInstance.Initialize())
				{
					//TODO: Notify process failure to start.
					return;
				}

				UIService.Invoke(() =>
				{
					CreateConsoleView(consoleInstance);
				});
			}));

			processInitThread.Start();
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
			consoleInstance.ShowProcessView();
		}

		private void TerminateConsoleInstance(IConsoleInstance consoleInstance)
		{
			consoleInstance.Terminate();
		}

		private void OnConsoleInstanceProcessTerminated(object sender, EventArgs args)
		{
			var instance = sender as IConsoleInstance;

			if (instance != null)
			{
				UIService.Invoke(() =>
				{
					HiddenConsoleInstances.Remove(instance);
					DestroyConsoleView(instance);
				});
			}
		}

		private void CreateConsoleView(IConsoleInstance consoleInstance)
		{
			var consoleViewModel = _consoleService.CreateConsoleViewModel(_shellViewHandle, consoleInstance);

			_consoleViewModels.Add(consoleInstance.ProcessId, consoleViewModel);
			_consoleService.ShowConsoleView(ViewHandle, consoleViewModel);
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
