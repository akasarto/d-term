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
		IReactiveDerivedList<IProcessViewModel> Processes { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesCommand { get; }
	}

	/// <summary>
	/// App processes panel view model implementation.
	/// <seealso cref="IProcessesPanelViewModel"/>
	/// </summary>
	public class ProcessesPanelViewModel : ReactiveObject, IProcessesPanelViewModel
	{
		//
		private readonly IProcessesRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _entities;

		//
		private bool _isLoadingOptions;
		private IReactiveDerivedList<IProcessViewModel> _processes;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadProcessesCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesPanelViewModel(IProcessesRepository processesRepository = null)
		{
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessesRepository>();

			_entities = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };

			_processes = _entities.CreateDerivedCollection(
				selector: option => Mapper.Map<IProcessViewModel>(option)
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
				_entities.Clear();
				_entities.AddRange(options);
			});
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
		/// Gets the load processes command.
		/// </summary>
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesCommand => _loadProcessesCommand;

		/// <summary>
		/// Gets the current available processes.
		/// </summary>
		public IReactiveDerivedList<IProcessViewModel> Processes => _processes;
	}
}
