using dTerm.Core;
using dTerm.Core.Processes;
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
		private IEnumerable<ConsoleDescriptor> _consoleDescriptors;
		private IConsoleInstanceFactory _consoleInstanceFactory;
		private IConsoleInstance _selectedConsoleInstance;

		public ShellViewModel(IEnumerable<ConsoleDescriptor> consoleDescriptors, IConsoleInstanceFactory consoleInstanceFactory)
		{
			_consoleDescriptors = consoleDescriptors ?? throw new ArgumentNullException(nameof(consoleDescriptors), nameof(ShellViewModel));
			_consoleInstanceFactory = consoleInstanceFactory ?? throw new ArgumentNullException(nameof(consoleInstanceFactory), nameof(ShellViewModel));

			//
			ConsoleInstances = new ObservableCollection<IConsoleInstance>();

			CreateConsoleProcessInstanceCommand = new RelayCommand<ConsoleDescriptor>(
				CreateConsoleProcessInstance,
				CanCreateConsoleProcessInstance
			);
		}

		public IEnumerable<ConsoleDescriptor> ConsoleDescriptors => _consoleDescriptors;

		public RelayCommand<ConsoleDescriptor> CreateConsoleProcessInstanceCommand { get; private set; }

		public ObservableCollection<IConsoleInstance> ConsoleInstances { get; private set; }

		public IConsoleInstance SelectedConsoleInstance
		{
			get => _selectedConsoleInstance;
			set => Set(ref _selectedConsoleInstance, value);
		}

		public IntPtr ShellViewHandle
		{
			get => _shellViewHandle;
			set => Set(ref _shellViewHandle, value);
		}

		private void CreateConsoleProcessInstance(ConsoleDescriptor descriptor)
		{
			var consoleInstance = _consoleInstanceFactory.CreateInstance(descriptor);

			consoleInstance.Killed += OnConsoleInstanceKilled;

			if (consoleInstance.Start())
			{
				Win32Api.TakeOwnership(
					consoleInstance.ProcessMainWindowHandle,
					ShellViewHandle
				);

				ConsoleInstances.Add(consoleInstance);
			}
		}

		private void OnConsoleInstanceKilled(object sender, EventArgs e)
		{
			var consoleInstance = sender as IConsoleInstance;

			if (consoleInstance == null)
			{
				throw new InvalidOperationException(nameof(OnConsoleInstanceKilled), new ArgumentException(nameof(consoleInstance), nameof(ShellViewModel)));
			}

			UIService.Invoke(() =>
			{
				ConsoleInstances.Remove(consoleInstance);
			});
		}

		private bool CanCreateConsoleProcessInstance(ConsoleDescriptor consoleOption)
		{
			if (consoleOption == null || consoleOption.ProcessStartInfo == null)
			{
				return true;
			}

			return consoleOption.IsSupported;
		}
	}
}
