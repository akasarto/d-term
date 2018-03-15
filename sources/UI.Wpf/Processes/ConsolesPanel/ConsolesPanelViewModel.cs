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

namespace UI.Wpf.Processes
{
	//
	public interface IConsolesPanelViewModel
	{
		bool IsLoadingConsoles { get; }
		bool StartProcessAsAdmin { get; }
		IReactiveDerivedList<IProcessViewModel> Options { get; }
		ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadOptionsCommand { get; }
		ReactiveCommand<IProcessViewModel, IInstanceViewModel> StartConsoleProcessCommand { get; }
	}

	//
	public class ConsolesPanelViewModel : ReactiveObject, IConsolesPanelViewModel
	{
		private readonly IInstancesManager _instancesManager;
		private readonly IProcessRepository _processesRepository;
		private readonly IProcessInstanceFactory _consoleProcessFactory;
		private readonly IReactiveList<ProcessEntity> _processEntitiesSource;
		private readonly IReactiveDerivedList<IProcessViewModel> _consoleOptions;
		private readonly Func<ReactiveCommand<IProcessViewModel, IInstanceViewModel>> _startConsoleProcessCommandFactory;
		private readonly ReactiveCommand<Unit, IEnumerable<ProcessEntity>> _loadOptionsCommand;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private bool _startProcessAsAdmin;
		private bool _isLoadingConsoles;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelViewModel(
			IInstancesManager instancesManager = null,
			IProcessRepository processesRepository = null,
			IProcessInstanceFactory consoleProcessFactory = null,
			ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_instancesManager = instancesManager ?? Locator.CurrentMutable.GetService<IInstancesManager>();
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();
			_consoleProcessFactory = consoleProcessFactory ?? Locator.CurrentMutable.GetService<IProcessInstanceFactory>();
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
				var command = ReactiveCommand.CreateFromTask<IProcessViewModel, IInstanceViewModel>(async (option) => await StartConsoleProcessCommandAction(option));
				command.ThrownExceptions.Subscribe(@exception => StartConsoleProcessCommandError(@exception));
				command.Subscribe(instance => StartConsoleProcessCommandHandler(instance));
				return command;
			};
		}

		public bool StartProcessAsAdmin
		{
			get => _startProcessAsAdmin;
			set => this.RaiseAndSetIfChanged(ref _startProcessAsAdmin, value);
		}

		public bool IsLoadingConsoles
		{
			get => _isLoadingConsoles;
			set => this.RaiseAndSetIfChanged(ref _isLoadingConsoles, value);
		}

		public IReactiveDerivedList<IProcessViewModel> Options => _consoleOptions;

		public ReactiveCommand<IProcessViewModel, IInstanceViewModel> StartConsoleProcessCommand => _startConsoleProcessCommandFactory();

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

		private Task<IInstanceViewModel> StartConsoleProcessCommandAction(IProcessViewModel option) => Task.Run(() =>
		{
			var instance = default(IInstanceViewModel);
			var process = _consoleProcessFactory.Create(option, _startProcessAsAdmin);
			if (process.Start())
			{
				instance = Mapper.Map<IInstanceViewModel>(process);
				instance = (IInstanceViewModel)Mapper.Map(
					option,
					instance,
					typeof(IProcessViewModel),
					typeof(IInstanceViewModel)
				);
			}
			if (instance == null)
			{
				process.Stop();
				process.Dispose();
			}
			return Task.FromResult(instance);
		});

		private void StartConsoleProcessCommandError(Exception exception)
		{
			// ToDo: Log exception
			_snackbarMessageQueue.Enqueue("Error creating instance. Please try again.");
		}

		private void StartConsoleProcessCommandHandler(IInstanceViewModel instance)
		{
			if (instance == null)
			{
				_snackbarMessageQueue.Enqueue("The process failed to start. Please try again.");
				return;
			}

			if (!Win32Api.IsConsoleClass(instance.ProcessMainWindowHandle))
			{
				_snackbarMessageQueue.Enqueue("The process is not a valid console application.");
				instance.TerminateProcess();
				return;
			}

			_instancesManager.Track(instance);
		}
	}
}
