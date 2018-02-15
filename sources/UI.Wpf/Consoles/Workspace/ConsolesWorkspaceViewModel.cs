using Consoles.Core;
using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		IConsolesRepository _tempRepo = null;

		//
		private readonly ProcessInstancesArrangeViewModel _processInstancesArrangeViewModel = null;
		private readonly ConsoleOptionsListViewModel _consoleOptionsListViewModel = null;
		private readonly ProcessInstancesListViewModel _processInstancesListViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(IConsolesRepository tempRepo, ProcessInstancesArrangeViewModel processInstancesArrangeViewModel, ConsoleOptionsListViewModel consoleOptionsListViewModel, ProcessInstancesListViewModel processInstancesListViewModel)
		{
			_tempRepo = tempRepo;
			_processInstancesArrangeViewModel = processInstancesArrangeViewModel ?? throw new ArgumentNullException(nameof(processInstancesArrangeViewModel), nameof(ConsolesWorkspaceViewModel));
			_consoleOptionsListViewModel = consoleOptionsListViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsListViewModel), nameof(ConsolesWorkspaceViewModel));
			_processInstancesListViewModel = processInstancesListViewModel ?? throw new ArgumentNullException(nameof(processInstancesListViewModel), nameof(ConsolesWorkspaceViewModel));

			SetupCommands();
		}

		/// <summary>
		/// Gets the process instances arrange view model.
		/// </summary>
		public ProcessInstancesArrangeViewModel ProcessInstancesArrangeViewModel => _processInstancesArrangeViewModel;

		/// <summary>
		/// Gets the console options list view model.
		/// </summary>
		public ConsoleOptionsListViewModel ConsoleOptionsListViewModel => _consoleOptionsListViewModel;

		/// <summary>
		/// Gets the console process instances list view model.
		/// </summary>
		public ProcessInstancesListViewModel ProcessInstancesListViewModel => _processInstancesListViewModel;

		/// <summary>
		/// Show the settings window.
		/// </summary>
		public ReactiveCommand ShowSettingsCommand { get; protected set; }

		/// <summary>
		/// Initializer the model.
		/// </summary>
		public void Initialize()
		{
		}

		/// <summary>
		/// Wire up commands with their respective actions.
		/// </summary>
		private void SetupCommands()
		{
			ShowSettingsCommand = ReactiveCommand.Create(() =>
			{
				var view = new ConsoleSettingsView()
				{
					Owner = Application.Current.MainWindow,
					DataContext = new ConsoleSettingsViewModel(_tempRepo),
					WindowStartupLocation = WindowStartupLocation.CenterOwner,
					ShowInTaskbar = false
				};

				view.ShowDialog();
			});
		}
	}
}
