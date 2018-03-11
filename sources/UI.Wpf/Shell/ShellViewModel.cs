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
		IConsolesViewModel ConsolesViewModel { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand OpenSettingsCommand { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		//
		private readonly IConsolesViewModel _consolesViewModel;
		private readonly Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private readonly ReactiveCommand _openSettingsReactiveCommand;
		private readonly ISettingsViewModel _settingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISettingsViewModel settingsViewModel = null, IConsolesViewModel consolesViewModel = null)
		{
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();
			_consolesViewModel = consolesViewModel ?? Locator.CurrentMutable.GetService<IConsolesViewModel>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			// Settings
			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					_consolesViewModel.LoadOptionsCommand.Execute().Subscribe();
				})
			);
		}

		public IConsolesViewModel ConsolesViewModel => _consolesViewModel;

		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		public ReactiveCommand OpenSettingsCommand => _openSettingsReactiveCommand;
	}
}
