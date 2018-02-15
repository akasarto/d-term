using AutoMapper;
using Consoles.Core;
using Consoles.Processes;
using Dragablz.Dockablz;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		IConsolesRepository _tempRepo = null;

		//
		private readonly ConsoleOptionsListViewModel _consoleOptionsListViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(IConsolesRepository tempRepo, ConsoleOptionsListViewModel consoleOptionsListViewModel)
		{
			_tempRepo = tempRepo;
			_consoleOptionsListViewModel = consoleOptionsListViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsListViewModel), nameof(ConsolesWorkspaceViewModel));

			SetupCommands();
		}

		/// <summary>
		/// Gets the console options list view model.
		/// </summary>
		public ConsoleOptionsListViewModel ConsoleOptionsListViewModel => _consoleOptionsListViewModel;

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
