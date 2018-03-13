using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using UI.Wpf.Processes;

namespace UI.Wpf.Shell
{
	//
	public interface IShellViewModel
	{
		IProcessesController Processes { get; }
		Interaction<IConfigsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand OpenSettingsCommand { get; }
		ISnackbarMessageQueue MessageQueue { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private readonly IProcessesController _processesController;
		
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private readonly Interaction<IConfigsViewModel, Unit> _openSettingsInteraction;
		private readonly ReactiveCommand _openSettingsReactiveCommand;
		private readonly IConfigsViewModel _settingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(IProcessesController processesController = null, ISnackbarMessageQueue snackbarMessageQueue = null, IConfigsViewModel settingsViewModel = null)
		{
			_processesController = processesController ?? Locator.CurrentMutable.GetService<IProcessesController>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<IConfigsViewModel>();

			_openSettingsInteraction = new Interaction<IConfigsViewModel, Unit>();

			// Settings
			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					//_consoleOptionsPanelViewModel.LoadOptionsCommand.Execute().Subscribe();
				})
			);
		}

		public IProcessesController Processes => _processesController;

		public Interaction<IConfigsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		public ReactiveCommand OpenSettingsCommand => _openSettingsReactiveCommand;

		public ISnackbarMessageQueue MessageQueue => _snackbarMessageQueue;
	}
}
