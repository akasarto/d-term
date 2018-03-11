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
	public interface IConsolesViewModel
	{
		bool IsLoadingConsoles { get; }
		IReactiveDerivedList<IConsoleOptionViewModel> Options { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand { get; }
		ReactiveCommand<IConsoleOptionViewModel, IConsoleInstanceViewModel> CreateProcessInstanceCommand { get; }
		IReactiveDerivedList<IConsoleInstanceViewModel> Instances { get; }
	}

	//
	public class ConsolesViewModel : ReactiveObject, IConsolesViewModel
	{
		private readonly IProcessRepository _processesRepository;
		private readonly IConsoleProcessFactory _consoleProcessFactory;
		private readonly IReactiveList<ProcessEntity> _processEntitiesSource;
		private readonly IReactiveList<IConsoleInstanceViewModel> _instancesSource;
		private readonly IReactiveDerivedList<IConsoleOptionViewModel> _consoleOptions;
		private readonly IReactiveDerivedList<IConsoleInstanceViewModel> _consoleInstances;
		private readonly Func<ReactiveCommand<IConsoleOptionViewModel, IConsoleInstanceViewModel>> _createConsoleInstanceCommandFactory;
		private readonly ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private bool _isLoadingConsoles;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesViewModel(IProcessRepository processesRepository = null, IConsoleProcessFactory consoleProcessFactory = null)
		{
			//
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();
			_consoleProcessFactory = consoleProcessFactory ?? Locator.CurrentMutable.GetService<IConsoleProcessFactory>();

			//
			_processEntitiesSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };
			_instancesSource = new ReactiveList<IConsoleInstanceViewModel>() { ChangeTrackingEnabled = true };

			//
			_consoleOptions = _processEntitiesSource.CreateDerivedCollection(
				selector: process => Mapper.Map<IConsoleOptionViewModel>(process)
			);
			_consoleInstances = _instancesSource.CreateDerivedCollection(
				selector: instance => instance
			);

			/*
			 * Instances (Open / Close Windows)
			 */
			_consoleInstances.ItemsAdded.Subscribe(addedInstance =>
			{
				var mainHandle = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;

				Win32Api.RemoveFromTaskbar(addedInstance.ProcessMainWindowHandle);
				Win32Api.TakeOwnership(addedInstance.ProcessMainWindowHandle, mainHandle);

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

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingConsoles);

			_loadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{

				// ToDo: Show message
			});

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
				var commandIntance = ReactiveCommand.CreateFromTask<IConsoleOptionViewModel, IConsoleInstanceViewModel>(async (option) => await Task.Run(() =>
				{
					var instance = default(IConsoleInstanceViewModel);

					var process = _consoleProcessFactory.Create(option);

					if (process.Start())
					{
						instance = Mapper.Map<IConsoleInstanceViewModel>(process);

						instance = (IConsoleInstanceViewModel)Mapper.Map(
							option,
							instance,
							typeof(IConsoleOptionViewModel),
							typeof(IConsoleInstanceViewModel)
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
							_instancesSource.Remove(instance);
						});

						_instancesSource.Add(instance);

						return;
					}

					// ToDo: Show startup error.
				});

				return commandIntance;
			};
		}

		public bool IsLoadingConsoles
		{
			get => _isLoadingConsoles;
			set => this.RaiseAndSetIfChanged(ref _isLoadingConsoles, value);
		}

		public IReactiveDerivedList<IConsoleOptionViewModel> Options => _consoleOptions;

		public ReactiveCommand<IConsoleOptionViewModel, IConsoleInstanceViewModel> CreateProcessInstanceCommand => _createConsoleInstanceCommandFactory();

		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;

		public IReactiveDerivedList<IConsoleInstanceViewModel> Instances => _consoleInstances;
	}
}
