using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using UI.Wpf.Consoles;
using UI.Wpf.Settings;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// Shell view model interface.
	/// </summary>
	public interface IShellViewModel
	{
		ReactiveCommand OpenSettingsCommand { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		IConsoleOptionsPanelViewModel ConsolesPanelViewModel { get; }
	}

	/// <summary>
	/// App shell view model implementation.
	/// <seealso cref="IShellViewModel"/>
	/// </summary>
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		//
		private readonly ReactiveCommand _openSettingsCommand;
		private readonly IConsoleOptionsPanelViewModel _consoleOptionsPanelViewModel;
		private readonly Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private readonly ISettingsViewModel _settingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(IConsoleOptionsPanelViewModel consoleOptionsPanelViewModel = null, ISettingsViewModel settingsViewModel = null)
		{
			_consoleOptionsPanelViewModel = consoleOptionsPanelViewModel ?? Locator.CurrentMutable.GetService<IConsoleOptionsPanelViewModel>();
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			_openSettingsCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe()
			);
		}

		/// <summary>
		/// Gets the app general settings command instance.
		/// </summary>
		public ReactiveCommand OpenSettingsCommand => _openSettingsCommand;

		/// <summary>
		/// Gets the interaction that opens the general settings window.
		/// </summary>
		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		/// <summary>
		/// Gets or sets the console options panel view model.
		/// </summary>
		public IConsoleOptionsPanelViewModel ConsolesPanelViewModel => _consoleOptionsPanelViewModel;
	}
}
