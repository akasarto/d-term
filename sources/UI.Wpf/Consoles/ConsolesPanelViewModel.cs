using AutoMapper;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;
using WinApi.User32;

namespace UI.Wpf.Consoles
{
	//
	public interface IConsolesPanelViewModel
	{
		bool IsLoadingProcesses { get; }
		IReactiveDerivedList<IConsoleOptionViewModel> ProcessOptions { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand { get; }
		Interaction<IProcessInstanceViewModel, IntPtr> OpenProcessInstanceViewInteraction { get; }
		ReactiveCommand<IConsoleOptionViewModel, IProcessInstanceViewModel> CreateProcessInstanceCommand { get; }
		IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances { get; }
		Interaction<IntPtr, bool> CloseProcessInstanceViewInteraction { get; }
	}

	//
	public class ConsolesPanelViewModel : ReactiveObject, IConsolesPanelViewModel
	{
		private readonly IConsoleProcessFactory _processFactory;
		private readonly IProcessRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _processOptionsSource;
		private readonly IReactiveDerivedList<IConsoleOptionViewModel> _processOptions;
		private readonly IReactiveList<IProcessInstanceViewModel> _processInstancesSource;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _processInstances;
		private readonly Func<ReactiveCommand<IConsoleOptionViewModel, IProcessInstanceViewModel>> _createProcessInstanceCommandFactory;
		private readonly Interaction<IProcessInstanceViewModel, IntPtr> _openProcessInstanceViewInteraction;
		private readonly Interaction<IntPtr, bool> _closeProcessInstanceViewInteraction;
		private readonly ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private bool _isLoadingOptions;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelViewModel(IConsoleProcessFactory processFactory = null, IProcessRepository processesRepository = null)
		{
			//
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IConsoleProcessFactory>();
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();

			//
			_closeProcessInstanceViewInteraction = new Interaction<IntPtr, bool>();
			_openProcessInstanceViewInteraction = new Interaction<IProcessInstanceViewModel, IntPtr>();

			//
			_processOptionsSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_processInstancesSource = new ReactiveList<IProcessInstanceViewModel>() { ChangeTrackingEnabled = true };

			//
			_processOptions = _processOptionsSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IConsoleOptionViewModel>(process)
			);
			_processInstances = _processInstancesSource.CreateDerivedCollection(
				selector: instance => instance
			);

			/*
			 * Instances (Open / Close Windows)
			 */
			_processInstances.ItemsAdded.Subscribe(addedInstance =>
			{
				Win32Api.TakeOwnership(addedInstance.ProcessMainWindowHandle, new WindowInteropHelper(App.Current.MainWindow).Handle);

				if (!User32Methods.IsWindowVisible(addedInstance.ProcessMainWindowHandle))
				{
					User32Methods.ShowWindow(addedInstance.ProcessMainWindowHandle, ShowWindowCommands.SW_SHOW);
				}
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
				var commandIntance = ReactiveCommand.CreateFromTask<IConsoleOptionViewModel, IProcessInstanceViewModel>(async (option) => await Task.Run(() =>
				{
					var instance = default(IProcessInstanceViewModel);

					var process = _processFactory.Create(option);

					if (process.Start())
					{
						instance = Mapper.Map<IProcessInstanceViewModel>(process);

						instance = (IProcessInstanceViewModel)Mapper.Map(
							option,
							instance,
							typeof(IConsoleOptionViewModel),
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
						var instanceSubscription = instance.ProcessTerminated.ObserveOnDispatcher().Subscribe(@event =>
						{
							_processInstancesSource.Remove(instance);
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

		public IReactiveDerivedList<IConsoleOptionViewModel> ProcessOptions => _processOptions;

		public ReactiveCommand<IConsoleOptionViewModel, IProcessInstanceViewModel> CreateProcessInstanceCommand => _createProcessInstanceCommandFactory();

		public Interaction<IProcessInstanceViewModel, IntPtr> OpenProcessInstanceViewInteraction => _openProcessInstanceViewInteraction;

		public Interaction<IntPtr, bool> CloseProcessInstanceViewInteraction => _closeProcessInstanceViewInteraction;

		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;

		public IReactiveDerivedList<IProcessInstanceViewModel> ProcessInstances => _processInstances;
	}
}
