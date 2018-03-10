using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using UI.Wpf.Consoles;
using UI.Wpf.Settings;

namespace UI.Wpf.Shell
{
	//
	public interface IShellViewModel
	{
		IConsolesPanelViewModel ConsolesPanelViewModel { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand OpenSettingsCommand { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		//
		private readonly IConsolesPanelViewModel _consolesPanelViewModel;
		private readonly Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private readonly ReactiveCommand _openSettingsReactiveCommand;
		private readonly ISettingsViewModel _settingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISettingsViewModel settingsViewModel = null, IConsolesPanelViewModel consolesPanelViewModel = null)
		{
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();
			_consolesPanelViewModel = consolesPanelViewModel ?? Locator.CurrentMutable.GetService<IConsolesPanelViewModel>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			/*
			 * Settings
			 */
			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					_consolesPanelViewModel.LoadOptionsCommand.Execute().Subscribe();
				})
			);
		}

		public IConsolesPanelViewModel ConsolesPanelViewModel => _consolesPanelViewModel;

		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		public ReactiveCommand OpenSettingsCommand => _openSettingsReactiveCommand;
	}
}
