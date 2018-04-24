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
	public interface IConsolesPanelViewModel
	{
		IAppState AppState { get; }
		bool IsLoadingConsoles { get; }
		bool StartProcessAsAdmin { get; }
		IReactiveDerivedList<IProcessViewModel> Consoles { get; }
		ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadConsolesCommand { get; }
		ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> StartConsoleProcessCommand { get; }
	}

	//
	public class ConsolesPanelViewModel : ReactiveObject, IConsolesPanelViewModel
	{
		private readonly IAppState _appState;
		private readonly IProcessFactory _processFactory;
		private readonly IProcessesTracker _processesTracker;
		private readonly IProcessRepository _processesRepository;
		private readonly IProcessesInteropAgent _consolesInteropAgent;
		private readonly IReactiveList<ProcessEntity> _consoleProcessEntities;
		private readonly IReactiveDerivedList<IProcessViewModel> _consolesList;
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel>> _startConsoleProcessCommandFactory;
		private readonly ReactiveCommand<Unit, IEnumerable<ProcessEntity>> _loadConsolesCommand;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private bool _isLoadingConsoles;
		private bool _startAsAdmin;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelViewModel(
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
			_consoleProcessEntities = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_consolesList = _consoleProcessEntities.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);

			// Load Processess
			_loadConsolesCommand = ReactiveCommand.CreateFromTask(async () => await LoadConsolesCommandAction());
			_loadConsolesCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingConsoles);
			_loadConsolesCommand.Subscribe(entities => LoadConsolesCommandHandler(entities));

			// Create Instances
			_startConsoleProcessCommandFactory = () =>
			{
				var command = ReactiveCommand.CreateFromTask<IProcessViewModel, IProcessInstanceViewModel>(async (option) => await StartConsoleProcessCommandAction(option));
				command.ThrownExceptions.Subscribe(@exception => StartConsoleProcessCommandError(@exception));
				command.Subscribe(instance => StartConsoleProcessCommandHandler(instance));
				return command;
			};
		}

		public bool StartProcessAsAdmin
		{
			get => _startAsAdmin;
			set => this.RaiseAndSetIfChanged(ref _startAsAdmin, value);
		}

		public bool IsLoadingConsoles
		{
			get => _isLoadingConsoles;
			set => this.RaiseAndSetIfChanged(ref _isLoadingConsoles, value);
		}

		public IAppState AppState => _appState;

		public IReactiveDerivedList<IProcessViewModel> Consoles => _consolesList;

		public ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> StartConsoleProcessCommand => _startConsoleProcessCommandFactory();

		public ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadConsolesCommand => _loadConsolesCommand;

		private Task<IEnumerable<ProcessEntity>> LoadConsolesCommandAction() => Task.Run(() =>
		{
			var items = _processesRepository.GetAllConsoles();
			return Task.FromResult(items);
		});

		private void LoadConsolesCommandHandler(IEnumerable<ProcessEntity> options)
		{
			_consoleProcessEntities.Clear();
			_consoleProcessEntities.AddRange(options);
		}

		private Task<IProcessInstanceViewModel> StartConsoleProcessCommandAction(IProcessViewModel processViewModel) => Task.Run(() =>
		{
			var instance = default(IProcessInstanceViewModel);
			var processInstance = _processFactory.Create(processViewModel, _startAsAdmin);

			if (processInstance.Start())
			{
				instance = Mapper.Map<IProcess, IProcessInstanceViewModel>(processInstance, context => context.AfterMap((source, target) =>
				{
					target = Mapper.Map(processViewModel, target);
					target.IsElevated = _startAsAdmin;
				}));

				_processesTracker.Track(processInstance.Id);

				return Task.FromResult(instance);
			}

			if (!processInstance.HasExited)
			{
				processInstance.Kill();
			}

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

			if (!Win32Api.IsConsoleClass(instance.ProcessMainWindowHandle))
			{
				_snackbarMessageQueue.Enqueue(Resources.InvalidConsoleProcess);
				instance.KillProcess();
				return;
			}

			_appState.AddProcessInstance(instance);
		}
	}
}
