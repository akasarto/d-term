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
		ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> CreateProcessInstanceCommand { get; }
		IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances { get; }
	}

	//
	public class ProcessesViewModel : ReactiveObject, IProcessesViewModel
	{
		private readonly IProcessFactory _processFactory;
		private readonly IProcessRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _processOptionsSource;
		private readonly IReactiveDerivedList<IProcessViewModel> _processOptions;
		private readonly IReactiveList<IProcessInstanceViewModel> _processInstancesSource;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _processInstances;
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel>> _createProcessInstanceCommandFactory;
		private readonly ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private bool _isLoadingOptions;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesViewModel(IProcessFactory processFactory = null, IProcessRepository processesRepository = null)
		{
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();

			_processOptionsSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_processInstancesSource = new ReactiveList<IProcessInstanceViewModel>() { ChangeTrackingEnabled = true };

			_processOptions = _processOptionsSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);

			_processInstances = _processInstancesSource.CreateDerivedCollection(
				selector: instance => instance
			);

			_processInstances.ItemsAdded.Subscribe(instance =>
			{
				User32Methods.ShowWindow(instance.MainWindowHandle, ShowWindowCommands.SW_SHOW);
			});

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
						instance.Terminated.ObserveOnDispatcher().Subscribe(@event =>
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

		/// <summary>
		/// Gets or sets the options loading status.
		/// </summary>
		public bool IsLoadingProcesses
		{
			get => _isLoadingOptions;
			set => this.RaiseAndSetIfChanged(ref _isLoadingOptions, value);
		}

		/// <summary>
		/// Gets the current available process options.
		/// </summary>
		public IReactiveDerivedList<IProcessViewModel> ProcessOptions => _processOptions;

		/// <summary>
		/// Gets the create process instance command.
		/// </summary>
		public ReactiveCommand<IProcessViewModel, IProcessInstanceViewModel> CreateProcessInstanceCommand => _createProcessInstanceCommandFactory();

		/// <summary>
		/// Gets the load processes command.
		/// </summary>
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available process instances.
		/// </summary>
		public IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances => _processInstances;
	}
}
