using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UI.Wpf.Processes;
using UI.Wpf.Settings;
using WinApi.User32;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// Shell view model interface.
	/// </summary>
	public interface IShellViewModel
	{
		ReactiveCommand OpenSettingsReactiveCommand { get; }
		Interaction<ISettingsViewModel, Unit> OpenSettingsInteraction { get; }
		ReactiveCommand<IProcessViewModel, IProcessInstance> CreateProcessInstanceCommand { get; }
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
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstance>> _createProcessInstanceCommandFactory;
		private readonly ReactiveCommand _openSettingsReactiveCommand;
		private readonly IProcessFactory _processFactory;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISettingsViewModel settingsViewModel = null, IProcessesPanelViewModel processesPanelViewModel = null, IProcessFactory processFactory = null)
		{
			_settingsViewModel = settingsViewModel ?? Locator.CurrentMutable.GetService<ISettingsViewModel>();
			_processesPanelViewModel = processesPanelViewModel ?? Locator.CurrentMutable.GetService<IProcessesPanelViewModel>();
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();

			_openSettingsInteraction = new Interaction<ISettingsViewModel, Unit>();

			/*
			 * Settings
			 */
			_openSettingsReactiveCommand = ReactiveCommand.Create(
				() => OpenSettingsInteraction.Handle(_settingsViewModel).Subscribe(result =>
				{
					_processesPanelViewModel.LoadProcessesCommand.Execute().Subscribe();
				})
			);

			/*
			 * Create Instances
			 */
			_createProcessInstanceCommandFactory = () =>
			{
				var commandIntance = ReactiveCommand.CreateFromTask<IProcessViewModel, IProcessInstance>(async (option) => await Task.Run(() =>
				{
					var instance = _processFactory.Create(option);

					instance.Start();

					return Task.FromResult(instance);
				}));

				commandIntance.ThrownExceptions.Subscribe(@exception =>
				{
					// ToDo: Show message
				});

				commandIntance.Subscribe(instance =>
				{
					if (instance.IsStarted)
					{
						User32Methods.ShowWindow(instance.MainWindowHandle, ShowWindowCommands.SW_SHOW);
						return;
					}

					instance.Dispose();
				});

				return commandIntance;
			};
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
		/// Gets the create process instance command.
		/// </summary>
		public ReactiveCommand<IProcessViewModel, IProcessInstance> CreateProcessInstanceCommand => _createProcessInstanceCommandFactory();

		/// <summary>
		/// Gets the app general settings command instance.
		/// </summary>
		public ReactiveCommand OpenSettingsReactiveCommand => _openSettingsReactiveCommand;
	}
}
