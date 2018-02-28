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
		ReactiveCommand OpenSettingsReactiveCommand { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		IProcessesPanelViewModel ProcessesPanelViewModel { get; }
	}

	/// <summary>
	/// App shell view model implementation.
	/// <seealso cref="IShellViewModel"/>
	/// </summary>
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		//
		private readonly ISettingsViewModel _settingsViewModel;
		private readonly IProcessesPanelViewModel _processesPanelViewModel;
		private readonly Interaction<ISettingsViewModel, Unit> _openSettingsInteraction;
		private readonly ReactiveCommand _openSettingsReactiveCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISettingsViewModel settingsViewModel = null, IProcessesPanelViewModel processesPanelViewModel = null)
		{
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();
			_processesPanelViewModel = processesPanelViewModel ?? Locator.CurrentMutable.GetService<IProcessesPanelViewModel>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					_processesPanelViewModel.LoadProcessesCommand.Execute().Subscribe();
				})
			);
		}

		/// <summary>
		/// Gets the processes panel view model instance.
		/// </summary>
		public IProcessesPanelViewModel ProcessesPanelViewModel => _processesPanelViewModel;

		/// <summary>
		/// Gets the interaction that opens the general settings window.
		/// </summary>
		public Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction => _openSettingsInteraction;

		/// <summary>
		/// Gets the app general settings command instance.
		/// </summary>
		public ReactiveCommand OpenSettingsReactiveCommand => _openSettingsReactiveCommand;
	}
}
