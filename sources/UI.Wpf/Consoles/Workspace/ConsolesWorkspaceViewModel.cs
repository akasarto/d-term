using Consoles.Core;
using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		IConsoleOptionsRepository _tempRepo = null;

		//
		private readonly ConsoleProcessInstancesArrangeViewModel _consoleProcessInstancesArrangeViewModel = null;
		private readonly ConsoleOptionsListViewModel _consoleOptionsListViewModel = null;
		private readonly ConsoleProcessInstancesListViewModel _consoleProcessInstancesListViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(IConsoleOptionsRepository tempRepo, ConsoleProcessInstancesArrangeViewModel consoleProcessInstancesArrangeViewModel, ConsoleOptionsListViewModel consoleOptionsListViewModel, ConsoleProcessInstancesListViewModel consoleProcessInstancesListViewModel)
		{
			_tempRepo = tempRepo;
			_consoleProcessInstancesArrangeViewModel = consoleProcessInstancesArrangeViewModel ?? throw new ArgumentNullException(nameof(consoleProcessInstancesArrangeViewModel), nameof(ConsolesWorkspaceViewModel));
			_consoleOptionsListViewModel = consoleOptionsListViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsListViewModel), nameof(ConsolesWorkspaceViewModel));
			_consoleProcessInstancesListViewModel = consoleProcessInstancesListViewModel ?? throw new ArgumentNullException(nameof(consoleProcessInstancesListViewModel), nameof(ConsolesWorkspaceViewModel));

			SetupCommands();
		}

		/// <summary>
		/// Gets the process instances arrange view model.
		/// </summary>
		public ConsoleProcessInstancesArrangeViewModel ConsoleProcessInstancesArrangeViewModel => _consoleProcessInstancesArrangeViewModel;

		/// <summary>
		/// Gets the console options list view model.
		/// </summary>
		public ConsoleOptionsListViewModel ConsoleOptionsListViewModel => _consoleOptionsListViewModel;

		/// <summary>
		/// Gets the console process instances list view model.
		/// </summary>
		public ConsoleProcessInstancesListViewModel ConsoleProcessInstancesListViewModel => _consoleProcessInstancesListViewModel;

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
				var view = new ConsoleSettingsDialogView()
				{
					Owner = Application.Current.MainWindow,
					DataContext = new ConsoleSettingsDialogViewModel(_tempRepo),
					WindowStartupLocation = WindowStartupLocation.CenterOwner,
					ShowInTaskbar = false
				};

				view.ShowDialog();
			});
		}
	}
}
