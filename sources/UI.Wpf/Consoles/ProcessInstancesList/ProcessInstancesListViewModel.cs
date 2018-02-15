using AutoMapper;
using Consoles.Core;
using Dragablz.Dockablz;
using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ProcessInstancesListViewModel : BaseViewModel
	{
		private IInputElement _consolesControl;
		private ArrangeOption _currentArrangeOption = ArrangeOption.Grid;
		private ReactiveList<IConsoleProcess> _consoleProcesses;
		private IReactiveDerivedList<ConsoleProcessInstanceViewModel> _consoleInstanceViewModels;

		//
		private readonly IConsolesProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstancesListViewModel(IConsolesProcessService consolesProcessService)
		{
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(ConsoleOptionViewModel));

			_consoleProcesses = new ReactiveList<IConsoleProcess>();

			Instances = _consoleProcesses.CreateDerivedCollection(
				filter: consoleProcess => true,
				selector: consoleProcess => Mapper.Map<ConsoleProcessInstanceViewModel>(consoleProcess)
			);

			Instances.Changed.Subscribe(instances => ArrangeProcessInstances());

			SetupMessageBus();
		}

		/// <summary>
		/// Gets or setsh the current console instances list.
		/// </summary>
		public IReactiveDerivedList<ConsoleProcessInstanceViewModel> Instances
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
		private void ArrangeProcessInstances()
		{
			switch (_currentArrangeOption)
			{
				case ArrangeOption.Grid:
					Layout.TileFloatingItemsCommand.Execute(null, _consolesControl);
					break;
				case ArrangeOption.Horizontally:
					Layout.TileFloatingItemsVerticallyCommand.Execute(null, _consolesControl);
					break;
				case ArrangeOption.Vertically:
					Layout.TileFloatingItemsHorizontallyCommand.Execute(null, _consolesControl);
					break;
			}

			ArrangeProcessInstances();
		}

		/// <summary>
		/// Wireup the message this view model wil listen to.
		/// </summary>
		private void SetupMessageBus()
		{
			MessageBus.Current.Listen<ArrangeChangedMessage>().Subscribe(message =>
			{
				_currentArrangeOption = message.NewArrange;
			});

			MessageBus.Current.Listen<CreateProcessInstanceMessage>().Subscribe(message =>
			{
				var process = message.Process;

				_consoleProcesses.Add(process);
			});
		}

		/// <summary>
		/// Event raised each time a console process is terminated.
		/// </summary>
		private void OnConsoleProcessTerminated(int processId)
		{
			//var instance = Instances.Where(i => i.ProcessId == processId).SingleOrDefault();

			//if (instance != null)
			//{
			//	Instances.Remove(instance);
			//}
		}
	}
}
