using AutoMapper;
using Consoles.Core;
using Dragablz.Dockablz;
using ReactiveUI;
using System;
using System.Linq;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessInstancesListViewModel : BaseViewModel
	{
		private IInputElement _consolesControl;
		private ConsoleArrangeOption _currentArrangeOption = ConsoleArrangeOption.Grid;
		private ReactiveList<IConsoleProcess> _consoleProcesses;
		private IReactiveDerivedList<ConsoleProcessInstanceViewModel> _consoleInstanceViewModels;

		//
		private readonly IConsoleProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessInstancesListViewModel(IConsoleProcessService consolesProcessService)
		{
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(ConsoleOptionViewModel));

			_consoleProcesses = new ReactiveList<IConsoleProcess>();

			ProcessInstances = _consoleProcesses.CreateDerivedCollection(
				filter: consoleProcess => true,
				selector: consoleProcess => Mapper.Map<ConsoleProcessInstanceViewModel>(consoleProcess),
				scheduler: RxApp.MainThreadScheduler
			);

			ProcessInstances.Changed.Subscribe(instances => ArrangeProcessInstances());

			SetupMessageBus();
		}

		/// <summary>
		/// Gets or sets the current console process instances.
		/// </summary>
		public IReactiveDerivedList<ConsoleProcessInstanceViewModel> ProcessInstances
		{
			get => _consoleInstanceViewModels;
			set => this.RaiseAndSetIfChanged(ref _consoleInstanceViewModels, value);
		}

		/// <summary>
		/// Initialize the model.
		/// </summary>
		public void Initialize(IInputElement consolesControl)
		{
			_consolesControl = consolesControl;
		}

		/// <summary>
		/// Auto arrange console process instances.
		/// </summary>
		public void ArrangeProcessInstances()
		{
			switch (_currentArrangeOption)
			{
				case ConsoleArrangeOption.Grid:
					Layout.TileFloatingItemsCommand.Execute(null, _consolesControl);
					break;

				case ConsoleArrangeOption.Horizontally:
					Layout.TileFloatingItemsVerticallyCommand.Execute(null, _consolesControl);
					break;

				case ConsoleArrangeOption.Vertically:
					Layout.TileFloatingItemsHorizontallyCommand.Execute(null, _consolesControl);
					break;
			}
		}

		/// <summary>
		/// Wireup the message this view model wil listen to.
		/// </summary>
		private void SetupMessageBus()
		{
			MessageBus.Current.Listen<ConsoleArrangeChangedMessage>().Subscribe(message =>
			{
				_currentArrangeOption = message.NewArrange;

				ArrangeProcessInstances();
			});

			MessageBus.Current.Listen<ConsoleProcessCreatedMessage>().Subscribe(message =>
			{
				var process = message.NewConsoleProcess;

				if (Win32Api.IsConsoleProcess(process.MainWindowHandle))
				{
					_consoleProcesses.Add(process);
				}
			});

			MessageBus.Current.Listen<ConsoleProcessTerminatedMessage>().Subscribe(message =>
			{
				var process = _consoleProcesses.Where(i => i.Id == message.TerminatedConsoleProcess.Id).SingleOrDefault();

				if (process != null)
				{
					_consoleProcesses.Remove(process);
				}
			});
		}
	}
}
