using AutoMapper;
using MaterialDesignThemes.Wpf;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UI.Wpf.Properties;

namespace UI.Wpf.Processes
{
	//
	public interface IUtilitiesPanelViewModel
	{
		bool IsLoadingUtilities { get; }
		IReactiveDerivedList<IProcessViewModel> Utilities { get; }
		ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadUtilitiesCommand { get; }
		ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> StartUtilityProcessCommand { get; }
	}

	//
	public class UtilitiesPanelViewModel : ReactiveObject, IUtilitiesPanelViewModel
	{
		private readonly IAppState _appState;
		private readonly IProcessFactory _processFactory;
		private readonly IProcessesTracker _processesTracker;
		private readonly IProcessRepository _processesRepository;
		private readonly IProcessesInteropAgent _consolesInteropAgent;
		private readonly IReactiveList<ProcessEntity> _utilityProcessEntities;
		private readonly IReactiveDerivedList<IProcessViewModel> _utilitiesList;
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel>> _startUtilityProcessCommandFactory;
		private readonly ReactiveCommand<Unit, IEnumerable<ProcessEntity>> _loadUtilitiesCommand;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private bool _isLoadingUtilities;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public UtilitiesPanelViewModel(
			IAppState appState = null,
			IProcessFactory processFactory = null,
			IProcessesTracker processesTracker = null,
			IProcessRepository processesRepository = null,
			IProcessesInteropAgent consolesInteropAgent = null,
			ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();
			_processesTracker = processesTracker ?? Locator.CurrentMutable.GetService<IProcessesTracker>();
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();
			_consolesInteropAgent = consolesInteropAgent ?? Locator.CurrentMutable.GetService<IProcessesInteropAgent>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();

			// Lists
			_utilityProcessEntities = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_utilitiesList = _utilityProcessEntities.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);

			// Load Processess
			_loadUtilitiesCommand = ReactiveCommand.CreateFromTask(async () => await LoadUtilitiesCommandAction());
			_loadUtilitiesCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingUtilities);
			_loadUtilitiesCommand.Subscribe(entities => LoadUtilitiesCommandHandler(entities));

			// Create Instances
			_startUtilityProcessCommandFactory = () =>
			{
				var command = ReactiveCommand.CreateFromTask<IProcessViewModel, IProcessInstanceViewModel>(async (option) => await StartConsoleProcessCommandAction(option));
				command.ThrownExceptions.Subscribe(@exception => StartConsoleProcessCommandError(@exception));
				command.Subscribe(instance => StartConsoleProcessCommandHandler(instance));
				return command;
			};
		}

		public bool IsLoadingUtilities
		{
			get => _isLoadingUtilities;
			set => this.RaiseAndSetIfChanged(ref _isLoadingUtilities, value);
		}

		public IReactiveDerivedList<IProcessViewModel> Utilities => _utilitiesList;

		public ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> StartUtilityProcessCommand => _startUtilityProcessCommandFactory();

		public ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadUtilitiesCommand => _loadUtilitiesCommand;

		private Task<IEnumerable<ProcessEntity>> LoadUtilitiesCommandAction() => Task.Run(() =>
		{
			var items = _processesRepository.GetAllUtilities();
			return Task.FromResult(items);
		});

		private void LoadUtilitiesCommandHandler(IEnumerable<ProcessEntity> options)
		{
			_utilityProcessEntities.Clear();
			_utilityProcessEntities.AddRange(options);
		}

		private Task<IProcessInstanceViewModel> StartConsoleProcessCommandAction(IProcessViewModel processViewModel) => Task.Run(() =>
		{
			var instance = default(IProcessInstanceViewModel);
			var processInstance = _processFactory.Create(processViewModel, false);

			if (processInstance.Start())
			{
				instance = Mapper.Map<IProcess, IProcessInstanceViewModel>(processInstance, context => context.AfterMap((source, target) =>
				{
					target = Mapper.Map(processViewModel, target);
				}));

				_processesTracker.Track(processInstance.Id);

				return Task.FromResult(instance);
			}

			processInstance.Kill();

			return Task.FromResult<IProcessInstanceViewModel>(null);
		});

		private void StartConsoleProcessCommandError(Exception exception)
		{
			// ToDo: Log exception
			_snackbarMessageQueue.Enqueue(Resources.ErrorCreatingInstance);
		}

		private void StartConsoleProcessCommandHandler(IProcessInstanceViewModel instance)
		{
			if (instance == null)
			{
				_snackbarMessageQueue.Enqueue(Resources.ProcessStartFailure);
				return;
			}

			if (Win32Api.IsConsoleClass(instance.ProcessMainWindowHandle))
			{
				_snackbarMessageQueue.Enqueue(Resources.InvalidUtilityProcess);
				instance.KillProcess();
				return;
			}

			_appState.AddProcessInstance(instance);
		}
	}
}
