using Consoles.Core;
using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository = null;
		private readonly ConsoleOptionsListViewModel _consoleOptionsListViewModel = null;
		private readonly ConsoleProcessInstancesArrangeViewModel _consoleProcessInstancesArrangeViewModel = null;
		private readonly ConsoleProcessInstancesListViewModel _consoleProcessInstancesListViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(
			IConsoleOptionsRepository consoleOptionsRepository,
			ConsoleOptionsListViewModel consoleOptionsListViewModel,
			ConsoleProcessInstancesArrangeViewModel consoleProcessInstancesArrangeViewModel,
			ConsoleProcessInstancesListViewModel consoleProcessInstancesListViewModel)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? throw new ArgumentNullException(nameof(consoleOptionsRepository), nameof(ConsolesWorkspaceViewModel));
			_consoleOptionsListViewModel = consoleOptionsListViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsListViewModel), nameof(ConsolesWorkspaceViewModel));
			_consoleProcessInstancesArrangeViewModel = consoleProcessInstancesArrangeViewModel ?? throw new ArgumentNullException(nameof(consoleProcessInstancesArrangeViewModel), nameof(ConsolesWorkspaceViewModel));
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
		public ReactiveCommand ShowSettingsDialogCommand { get; protected set; }

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
			//ToDo: Create dialgo view factory (v3 milestone)
			ShowSettingsDialogCommand = ReactiveCommand.Create(() =>
			{
				var view = new ConsoleSettingsDialogView()
				{
					Owner = Application.Current.MainWindow,
					DataContext = new ConsoleSettingsDialogViewModel(_consoleOptionsRepository),
					WindowStartupLocation = WindowStartupLocation.CenterOwner,
					ShowInTaskbar = false
				};

				view.Show();
			});
		}
	}
}
