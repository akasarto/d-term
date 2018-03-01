using AutoMapper;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessesViewModel
	{
		bool IsLoadingProcesses { get; }
		IReactiveDerivedList<IProcessViewModel> ProcessOptions { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand { get; }
		Interaction<IProcessInstanceViewModel, IntPtr> OpenProcessInstanceViewInteraction { get; }
		ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> CreateProcessInstanceCommand { get; }
		IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances { get; }
		Interaction<IntPtr, bool> CloseProcessInstanceViewInteraction { get; }
	}

	//
	public class ProcessesViewModel : ReactiveObject, IProcessesViewModel
	{
		private readonly IProcessFactory _processFactory;
		private readonly IProcessRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _processOptionsSource;
		private readonly IReactiveDerivedList<IProcessViewModel> _processOptions;
		private readonly Interaction<IntPtr, bool> _closeProcessInstanceViewInteraction;
		private readonly IReactiveList<IProcessInstanceViewModel> _processInstancesSource;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _processInstances;
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel>> _createProcessInstanceCommandFactory;
		private readonly Interaction<IProcessInstanceViewModel, IntPtr> _openProcessInstanceViewInteraction;
		private readonly ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private readonly Dictionary<int, IntPtr> _embeddedInstancesTracker;
		private bool _isLoadingOptions;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesViewModel(IProcessFactory processFactory = null, IProcessRepository processesRepository = null)
		{
			//
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();

			//
			_embeddedInstancesTracker = new Dictionary<int, IntPtr>();
			_openProcessInstanceViewInteraction = new Interaction<IProcessInstanceViewModel, IntPtr>();
			_closeProcessInstanceViewInteraction = new Interaction<IntPtr, bool>();

			//
			_processOptionsSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_processInstancesSource = new ReactiveList<IProcessInstanceViewModel>() { ChangeTrackingEnabled = true };

			//
			_processOptions = _processOptionsSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);
			_processInstances = _processInstancesSource.CreateDerivedCollection(
				selector: instance => instance
			);

			/*
			 * Instances (Open / Close Windows)
			 */
			_processInstances.ItemsAdded.Subscribe(addedInstance =>
			{
				if (Win32Api.IsConsoleProcess(addedInstance.MainWindowHandle))
				{
					_openProcessInstanceViewInteraction.Handle(addedInstance).Subscribe(windowHandle =>
					{
						if (!_embeddedInstancesTracker.ContainsKey(addedInstance.Pid))
						{
							_embeddedInstancesTracker.Add(addedInstance.Pid, windowHandle);
						}
					});

					return;
				}

				User32Methods.ShowWindow(addedInstance.MainWindowHandle, ShowWindowCommands.SW_SHOW);
			});

			_processInstances.ItemsRemoved.ObserveOnDispatcher().Subscribe(removedInstance => _openProcessInstanceViewInteraction.Handle(removedInstance).Subscribe(windowHandle =>
			{
				var instanceHandle = _embeddedInstancesTracker[removedInstance.Pid];

				_closeProcessInstanceViewInteraction.Handle(instanceHandle).Where(closed => !closed).Subscribe(closed =>
				{
					// ToDo: Log error closing view.
				});
			}));

			/*
			 * Load Processess
			 */
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _processesRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingProcesses);

			_loadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadOptionsCommand.Subscribe(options =>
			{
				_processOptionsSource.Clear();
				_processOptionsSource.AddRange(options);
			});

			/*
			 * Create Instances
			 */
			_createProcessInstanceCommandFactory = () =>
			{
				var commandIntance = ReactiveCommand.CreateFromTask<IProcessViewModel, IProcessInstanceViewModel>(async (option) => await Task.Run(() =>
				{
					var instance = default(IProcessInstanceViewModel);

					var process = _processFactory.Create(option);

					if (process.Start())
					{
						instance = Mapper.Map<IProcessInstanceViewModel>(process);

						instance = (IProcessInstanceViewModel)Mapper.Map(
							option,
							instance,
							typeof(IProcessViewModel),
							typeof(IProcessInstanceViewModel)
						);
					}

					if (instance == null)
					{
						process.Dispose();
					}

					return Task.FromResult(instance);
				}));

				commandIntance.ThrownExceptions.Subscribe(@exception =>
				{
					// ToDo: Show error message
				});

				commandIntance.Subscribe(instance =>
				{
					if (instance != null)
					{
						var instanceSubscription = instance.Terminated.ObserveOnDispatcher().Subscribe(@event =>
						{
							var process = @event.Sender as IProcess;

							if (process != null)
							{
								var terminatedInstance = _processInstancesSource.Where(i => i.Pid == process.Id).SingleOrDefault();

								if (terminatedInstance != null)
								{
									_processInstancesSource.Remove(terminatedInstance);
								}
							}
						});

						_processInstancesSource.Add(instance);

						return;
					}

					// ToDo: Show startup error.
				});

				return commandIntance;
			};
		}

		public bool IsLoadingProcesses
		{
			get => _isLoadingOptions;
			set => this.RaiseAndSetIfChanged(ref _isLoadingOptions, value);
		}

		public IReactiveDerivedList<IProcessViewModel> ProcessOptions => _processOptions;

		public ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> CreateProcessInstanceCommand => _createProcessInstanceCommandFactory();

		public Interaction<IProcessInstanceViewModel, IntPtr> OpenProcessInstanceViewInteraction => _openProcessInstanceViewInteraction;

		public Interaction<IntPtr, bool> CloseProcessInstanceViewInteraction => _closeProcessInstanceViewInteraction;

		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;

		public IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances => _processInstances;
	}
}
