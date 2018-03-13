using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using UI.Wpf.Processes;
using UI.Wpf.Settings;

namespace UI.Wpf.Shell
{
	//
	public interface IShellViewModel
	{
		IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand OpenSettingsCommand { get; }
		ISnackbarMessageQueue MessageQueue { get; }
		IMinimizedProcessesPanelViewModel ConsoleInstancesPanelViewModel { get; }
		ITransparencyManagerPanelViewModel TransparencyManagerPanelViewModel { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private readonly IConsoleOptionsPanelViewModel _consoleOptionsPanelViewModel;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private readonly Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private readonly IMinimizedProcessesPanelViewModel _consoleInstancesPanelViewModel;
		private readonly ITransparencyManagerPanelViewModel _transparencyManagerPanelViewModel;
		private readonly ReactiveCommand _openSettingsReactiveCommand;
		private readonly ISettingsViewModel _settingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(
			ISnackbarMessageQueue snackbarMessageQueue = null,
			ISettingsViewModel settingsViewModel = null,
			IConsoleOptionsPanelViewModel consoleOptionsPanelViewModel = null,
			IMinimizedProcessesPanelViewModel consoleInstancesPanelViewModel = null,
			ITransparencyManagerPanelViewModel transparencyManagerPanelViewModel = null)
		{
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();
			_consoleOptionsPanelViewModel = consoleOptionsPanelViewModel ?? Locator.CurrentMutable.GetService<IConsoleOptionsPanelViewModel>();
			_consoleInstancesPanelViewModel = consoleInstancesPanelViewModel ?? Locator.CurrentMutable.GetService<IMinimizedProcessesPanelViewModel>();
			_transparencyManagerPanelViewModel = transparencyManagerPanelViewModel ?? Locator.CurrentMutable.GetService<ITransparencyManagerPanelViewModel>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			// Settings
			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					_consoleOptionsPanelViewModel.LoadOptionsCommand.Execute().Subscribe();
				})
			);
		}

		public IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel => _consoleOptionsPanelViewModel;

		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		public ReactiveCommand OpenSettingsCommand => _openSettingsReactiveCommand;

		public ISnackbarMessageQueue MessageQueue => _snackbarMessageQueue;

		public IMinimizedProcessesPanelViewModel ConsoleInstancesPanelViewModel => _consoleInstancesPanelViewModel;

		public ITransparencyManagerPanelViewModel TransparencyManagerPanelViewModel => _transparencyManagerPanelViewModel;
	}
}
