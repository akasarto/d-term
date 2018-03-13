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
using System.Windows.Interop;
using UI.Wpf.Properties;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	//
	public interface IConsolesPanelViewModel
	{
		bool IsLoadingConsoles { get; }
		IReactiveDerivedList<IProcessViewModel> Options { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand { get; }
		ReactiveCommand<IProcessViewModel, IInstanceViewModel> CreateProcessInstanceCommand { get; }
	}

	//
	public class ConsolesPanelViewModel : ReactiveObject, IConsolesPanelViewModel
	{
		private readonly IAppState _appState;
		private readonly IProcessRepository _processesRepository;
		private readonly IProcessInstanceFactory _consoleProcessFactory;
		private readonly IReactiveList<ProcessEntity> _processEntitiesSource;
		private readonly IReactiveDerivedList<IProcessViewModel> _consoleOptions;
		private readonly Func<ReactiveCommand<IProcessViewModel, IInstanceViewModel>> _createConsoleInstanceCommandFactory;
		private readonly ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private bool _isLoadingConsoles;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelViewModel(IAppState appState = null, IProcessRepository processesRepository = null, IProcessInstanceFactory consoleProcessFactory = null, ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();
			_consoleProcessFactory = consoleProcessFactory ?? Locator.CurrentMutable.GetService<IProcessInstanceFactory>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();

			//
			_processEntitiesSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };

			//
			_consoleOptions = _processEntitiesSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);

			/*
			 * Instances (Open / Close Windows)
			 */
			_appState.GetAll().ItemsAdded.Subscribe(addedInstance =>
			{
				var mainHandle = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;

				Win32Api.RemoveFromTaskbar(addedInstance.ProcessMainWindowHandle);
				Win32Api.TakeOwnership(addedInstance.ProcessMainWindowHandle, mainHandle);
				User32Methods.SendMessage(addedInstance.ProcessMainWindowHandle, 0x80, new IntPtr(0), Resources.dTermIcon.Handle);
				User32Methods.SendMessage(addedInstance.ProcessMainWindowHandle, 0x80, new IntPtr(1), Resources.dTermIcon.Handle);
			});

			/*
			 * Load Processess
			 */
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _processesRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingConsoles);

			_loadOptionsCommand.Subscribe(options =>
			{
				_processEntitiesSource.Clear();
				_processEntitiesSource.AddRange(options);
			});

			/*
			 * Create Instances
			 */
			_createConsoleInstanceCommandFactory = () =>
			{
				var commandInstance = ReactiveCommand.CreateFromTask<IProcessViewModel, IInstanceViewModel>(async (option) => await Task.Run(() =>
				{
					var instance = default(IInstanceViewModel);

					var process = _consoleProcessFactory.Create(option);

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
				}));

				commandInstance.ThrownExceptions.Subscribe(@exception =>
				{
					// ToDo: Show error message
				});

				commandInstance.Subscribe(instance =>
				{
					if (instance == null)
					{
						_snackbarMessageQueue.Enqueue("The process failed to start. Please try again.");
						return;
					}

					if (!Win32Api.IsConsoleProcess(instance.ProcessMainWindowHandle))
					{
						_snackbarMessageQueue.Enqueue("The process is not a valid console application.");
						instance.TerminateProcess();
						return;
					}

					var instanceSubscription = instance.ProcessTerminated.ObserveOnDispatcher().Subscribe(@event =>
					{
						_appState.Remove(instance);
					});

					_appState.Add(instance);
				});

				return commandInstance;
			};
		}

		public bool IsLoadingConsoles
		{
			get => _isLoadingConsoles;
			set => this.RaiseAndSetIfChanged(ref _isLoadingConsoles, value);
		}

		public IReactiveDerivedList<IProcessViewModel> Options => _consoleOptions;

		public ReactiveCommand<IProcessViewModel, IInstanceViewModel> CreateProcessInstanceCommand => _createConsoleInstanceCommandFactory();

		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;
	}
}
