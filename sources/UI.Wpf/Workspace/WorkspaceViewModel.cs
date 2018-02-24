using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using UI.Wpf.Consoles;
using UI.Wpf.Properties;

namespace UI.Wpf.Workspace
{
	/// <summary>
	/// Workspace view model interface.
	/// </summary>
	public interface IWorkspaceViewModel
	{
		string AppTitle { get; set; }
		IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel { get; }
		ReactiveCommand<Unit, Window> WorkspaceSettingsCommand { get; }
	}

	/// <summary>
	/// App workspace view model implementation.
	/// <seealso cref="IWorkspaceViewModel"/>
	/// </summary>
	public class WorkspaceViewModel : ReactiveObject, IWorkspaceViewModel
	{
		//
		private readonly IConsoleOptionsPanelViewModel _consoleOptionsPanelViewModel;

		//
		private ReactiveCommand<Unit, Window> _workspaceSettingsCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public WorkspaceViewModel(IConsoleOptionsPanelViewModel consoleOptionsPanelViewModel)
		{
			_consoleOptionsPanelViewModel = consoleOptionsPanelViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsPanelViewModel), nameof(WorkspaceViewModel));

			WorkspaceSettingsCommandSetup();
		}

		/// <summary>
		/// Gets the app title.
		/// </summary>
		public string AppTitle { get; set; } = Resources.AppTitle;

		/// <summary>
		/// Gets or sets the console options panel view model.
		/// </summary>
		public IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel => _consoleOptionsPanelViewModel;

		/// <summary>
		/// Gets the main workspace settings command instance.
		/// </summary>
		public ReactiveCommand<Unit, Window> WorkspaceSettingsCommand
		{
			get => _workspaceSettingsCommand;
			set => this.RaiseAndSetIfChanged(ref _workspaceSettingsCommand, value);
		}

		/// <summary>
		/// Setup the main settings command actions and observables.
		/// </summary>
		private void WorkspaceSettingsCommandSetup()
		{
			WorkspaceSettingsCommand = ReactiveCommand.Create<Window>(() => 
			{
				var settingsView = new WorkspaceSettingsView();

				settingsView.Owner = Application.Current.MainWindow;
				settingsView.WindowStartupLocation = WindowStartupLocation.CenterOwner;

				return settingsView;
			});

			WorkspaceSettingsCommand.Subscribe(view =>
			{
				view.Show();
			});
		}
	}
}
