using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using UI.Wpf.Consoles;
using UI.Wpf.Properties;
using UI.Wpf.Settings;

namespace UI.Wpf.Workspace
{
	/// <summary>
	/// Workspace view model interface.
	/// </summary>
	public interface IWorkspaceViewModel : IRoutableViewModel
	{
		string AppTitle { get; set; }
		IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand WorkspaceSettingsCommand { get; }
	}

	/// <summary>
	/// App workspace view model implementation.
	/// <seealso cref="IWorkspaceViewModel"/>
	/// </summary>
	public class WorkspaceViewModel : ReactiveObject, IWorkspaceViewModel
	{
		//
		private readonly IConsoleOptionsPanelViewModel _consoleOptionsPanelViewModel;
		private readonly ISettingsViewModel _settingsViewModel;
		private readonly IScreen _shellScreen;

		//
		private Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private ReactiveCommand _workspaceSettingsCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		//public WorkspaceViewModel(IConsoleOptionsPanelViewModel consoleOptionsPanelViewModel, ISettingsViewModel settingsViewModel, IShellScreen shellScreen)
		public WorkspaceViewModel(IShellScreen shellScreen = null)
		{
			//_consoleOptionsPanelViewModel = consoleOptionsPanelViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsPanelViewModel), nameof(WorkspaceViewModel));
			//_settingsViewModel = settingsViewModel ?? throw new ArgumentNullException(nameof(settingsViewModel), nameof(WorkspaceViewModel));
			_shellScreen = shellScreen ?? Locator.CurrentMutable.GetService<IShellScreen>();

			//_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			//WorkspaceSettingsCommandSetup();
		}

		/// <summary>
		/// Gets the app title.
		/// </summary>
		public string AppTitle { get; set; } = Resources.AppTitle;

		/// <summary>
		/// Gets the interaction that opens the general settings window.
		/// </summary>
		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		/// <summary>
		/// Gets or sets the console options panel view model.
		/// </summary>
		public IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel => _consoleOptionsPanelViewModel;

		/// <summary>
		/// Gets the main workspace settings command instance.
		/// </summary>
		public ReactiveCommand WorkspaceSettingsCommand
		{
			get => _workspaceSettingsCommand;
			set => this.RaiseAndSetIfChanged(ref _workspaceSettingsCommand, value);
		}

		public string UrlPathSegment => "Workspace";

		public IScreen HostScreen => _shellScreen;

		/// <summary>
		/// Setup the main settings command actions and observables.
		/// </summary>
		private void WorkspaceSettingsCommandSetup()
		{
			WorkspaceSettingsCommand = ReactiveCommand.Create(() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
			{

			}));
		}
	}
}
