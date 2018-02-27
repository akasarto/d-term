using AutoMapper;
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
	/// <summary>
	/// Processes panel view model interface.
	/// </summary>
	public interface IProcessesPanelViewModel
	{
		bool IsLoadingOptions { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsReactiveCommand { get; }
		IReactiveDerivedList<IProcessViewModel> ConsolesReactiveDerivedList { get; }
	}

	/// <summary>
	/// App processes panel view model implementation.
	/// <seealso cref="IProcessesPanelViewModel"/>
	/// </summary>
	public class ProcessesPanelViewModel : ReactiveObject, IProcessesPanelViewModel
	{
		//
		private readonly IProcessesRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _processes;

		//
		private bool _isLoadingOptions;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private IReactiveDerivedList<IProcessViewModel> _consoles;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesPanelViewModel(IProcessesRepository processesRepository = null)
		{
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessesRepository>();

			_processes = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = false };

			_consoles = _processes.CreateDerivedCollection(
				selector: option => Mapper.Map<IProcessViewModel>(option)
			);

			LoadOptionsCommandSetup();
		}

		/// <summary>
		/// Gets or sets the options loading status.
		/// </summary>
		public bool IsLoadingOptions
		{
			get => _isLoadingOptions;
			set => this.RaiseAndSetIfChanged(ref _isLoadingOptions, value);
		}

		/// <summary>
		/// Gets the load options command instance.
		/// </summary>
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsReactiveCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<IProcessViewModel> ConsolesReactiveDerivedList => _consoles;

		/// <summary>
		/// Setup the load comand actions and observables.
		/// </summary>
		private void LoadOptionsCommandSetup()
		{
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _processesRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingOptions);

			_loadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadOptionsCommand.Subscribe(options =>
			{
				_processes.Clear();
				_processes.AddRange(options);
			});
		}
	}
}
