using AutoMapper;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Processes panel view model interface.
	/// </summary>
	public interface IProcessesPanelViewModel
	{
		bool IsLoadingProcesses { get; }
		IReactiveDerivedList<IProcessViewModel> ProcessOptions { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesCommand { get; }
		ReactiveCommand<IProcessViewModel, IProcessInstance> CreateProcessInstanceCommand { get; }
		IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances { get; }
	}

	/// <summary>
	/// App processes panel view model implementation.
	/// <seealso cref="IProcessesPanelViewModel"/>
	/// </summary>
	public class ProcessesPanelViewModel : ReactiveObject, IProcessesPanelViewModel
	{
		//
		private readonly IProcessesRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _processOptionsSource;
		private readonly IReactiveList<IProcessInstance> _processInstancesSource;

		//
		private bool _isLoadingOptions;
		private IReactiveDerivedList<IProcessViewModel> _processOptions;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadProcessesCommand;
		private readonly Func<ReactiveCommand<IProcessViewModel, IProcessInstance>> _createProcessInstanceCommandFactory;
		private IReactiveDerivedList<IProcessInstanceViewModel> _processInstances;
		private readonly IProcessFactory _processFactory;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesPanelViewModel(IProcessesRepository processesRepository = null, IProcessFactory processFactory = null)
		{
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessesRepository>();
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();

			_processOptionsSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_processInstancesSource = new ReactiveList<IProcessInstance>() { ChangeTrackingEnabled = true };

			_processOptions = _processOptionsSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IProcessViewModel>(process)
			);

			_processInstances = _processInstancesSource.CreateDerivedCollection(
				selector: instance => Mapper.Map<IProcessInstanceViewModel>(instance)
			);

			/*
			 * Load Processess
			 */
			_loadProcessesCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _processesRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadProcessesCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingProcesses);

			_loadProcessesCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadProcessesCommand.Subscribe(options =>
			{
				_processOptionsSource.Clear();
				_processOptionsSource.AddRange(options);
			});

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
		public ReactiveCommand<IProcessViewModel, IProcessInstance> CreateProcessInstanceCommand => _createProcessInstanceCommandFactory();

		/// <summary>
		/// Gets the load processes command.
		/// </summary>
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesCommand => _loadProcessesCommand;

		/// <summary>
		/// Gets the current available process instances.
		/// </summary>
		public IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances => _processInstances;
	}
}
