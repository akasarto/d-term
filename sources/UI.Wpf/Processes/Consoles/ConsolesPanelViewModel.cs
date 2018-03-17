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
		IReactiveDerivedList<IProcessViewModel> Options { get; }
		ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadOptionsCommand { get; }
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
		private readonly IReactiveList<ProcessEntity> _processEntitiesSource;
		private readonly IReactiveDerivedList<IProcessViewModel> _consoleOptions;
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel>> _startConsoleProcessCommandFactory;
		private readonly ReactiveCommand<Unit, IEnumerable<ProcessEntity>> _loadOptionsCommand;
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
			_processEntitiesSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_consoleOptions = _processEntitiesSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);

			// Load Processess
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await LoadOptionsCommandAction());
			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingConsoles);
			_loadOptionsCommand.Subscribe(entities => LoadOptionsCommandHandler(entities));

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

		public IReactiveDerivedList<IProcessViewModel> Options => _consoleOptions;

		public ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> StartConsoleProcessCommand => _startConsoleProcessCommandFactory();

		public ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;

		private Task<IEnumerable<ProcessEntity>> LoadOptionsCommandAction() => Task.Run(() =>
		{
			var items = _processesRepository.GetAll();
			return Task.FromResult(items);
		});

		private void LoadOptionsCommandHandler(IEnumerable<ProcessEntity> options)
		{
			_processEntitiesSource.Clear();
			_processEntitiesSource.AddRange(options);
		}

		private Task<IProcessInstanceViewModel> StartConsoleProcessCommandAction(IProcessViewModel option) => Task.Run(() =>
		{
			var instance = default(IProcessInstanceViewModel);
			var process = _processFactory.Create(option, _startAsAdmin);

			if (process.Start())
			{
				instance = Mapper.Map<IProcess, IProcessInstanceViewModel>(process, context => context.AfterMap((source, target) =>
				{
					target = Mapper.Map(option, target);
					target.IsElevated = _startAsAdmin;
				}));

				_processesTracker.Track(process.Id);

				return Task.FromResult(instance);
			}

			process.Kill();

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
