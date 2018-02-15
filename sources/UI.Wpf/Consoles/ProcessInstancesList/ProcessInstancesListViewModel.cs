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
		private ReactiveList<ConsoleIProcessInstanceViewModel> _consoleInstanceViewModels;

		//
		private readonly IConsolesProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstancesListViewModel(IConsolesProcessService consolesProcessService)
		{
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(ConsoleOptionViewModel));

			_consoleInstanceViewModels = new ReactiveList<ConsoleIProcessInstanceViewModel>();

			_consoleInstanceViewModels.Changed.Subscribe(instances => ArrangeProcessInstances());

			SetupMessageBus();
		}

		/// <summary>
		/// Gets or setsh the current console instances list.
		/// </summary>
		public ReactiveList<ConsoleIProcessInstanceViewModel> Instances
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

			MessageBus.Current.Listen<CreateConsoleInstanceMessage>().Subscribe(message =>
			{
				var descriptor = message.ProcessDescriptor;

				var consoleProcess = _consolesProcessService.Create(descriptor);

				var consoleInstanceViewModel = new ConsoleIProcessInstanceViewModel(consoleProcess)
				{
					Name = descriptor.Name
				};

				Instances.Add(consoleInstanceViewModel);
			});
		}
	}
}
