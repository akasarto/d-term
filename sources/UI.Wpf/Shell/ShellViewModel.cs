using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using UI.Wpf.Processes;
using UI.Wpf.Settings;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// Shell view model interface.
	/// </summary>
	public interface IShellViewModel
	{
		IProcessesViewModel ProcessesViewModel { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand OpenSettingsCommand { get; }
	}

	/// <summary>
	/// App shell view model implementation.
	/// <seealso cref="IShellViewModel"/>
	/// </summary>
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		//
		private readonly IProcessesViewModel _processesViewModel;
		private readonly Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private readonly ReactiveCommand _openSettingsReactiveCommand;
		private readonly ISettingsViewModel _settingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISettingsViewModel settingsViewModel = null, IProcessesViewModel processesPanelViewModel = null)
		{
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();
			_processesViewModel = processesPanelViewModel ?? Locator.CurrentMutable.GetService<IProcessesViewModel>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			/*
			 * Settings
			 */
			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					_processesViewModel.LoadOptionsCommand.Execute().Subscribe();
				})
			);
		}

		/// <summary>
		/// Gets the processes view model.
		/// </summary>
		public IProcessesViewModel ProcessesViewModel => _processesViewModel;

		/// <summary>
		/// Gets the interaction that opens the general settings window.
		/// </summary>
		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		/// <summary>
		/// Gets the open settings command.
		/// </summary>
		public ReactiveCommand OpenSettingsCommand => _openSettingsReactiveCommand;
	}
}
