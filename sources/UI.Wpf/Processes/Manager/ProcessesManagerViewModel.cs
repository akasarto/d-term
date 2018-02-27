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
	/// Processes manager view model interface.
	/// </summary>
	public interface IProcessesManagerViewModel
	{
		bool IsLoadingProcesses { get; }
		ReactiveCommand AddProcessReactiveCommand { get; }
		ReactiveCommand CancelOperationReactiveCommand { get; }
		ReactiveCommand SaveOperationReactiveCommand { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesReactiveCommand { get; }
		IReactiveDerivedList<IProcessViewModel> ProcessesReactiveDerivedList { get; }
		IProcessViewModel FormData { get; set; }
	}

	/// <summary>
	/// App processes manager view model implementation.
	/// </summary>
	public class ProcessesManagerViewModel : ReactiveObject, IProcessesManagerViewModel
	{
		//
		private readonly IReactiveList<ProcessEntity> _entities;
		private readonly IProcessesRepository _processesRepository;

		//
		private bool _isLoadingProcesses;
		//private IDisposable _onCancelEventDisposable;
		private ReactiveCommand _addProcessReactiveCommand;
		private ReactiveCommand _cancelOperationReactiveCommand;
		private ReactiveCommand _saveOperationReactiveCommand;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsReactiveCommand;
		private IReactiveDerivedList<IProcessViewModel> _processesReactiveDerivedList;
		private IProcessViewModel _formData;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerViewModel(IProcessesRepository processesRepository = null)
		{
			var locator = Locator.CurrentMutable;

			_processesRepository = processesRepository ?? locator.GetService<IProcessesRepository>();

			_entities = new ReactiveList<ProcessEntity>();

			_processesReactiveDerivedList = _entities.CreateDerivedCollection(
				selector: option => Mapper.Map<IProcessViewModel>(option)
			);

			//_onCancelEventDisposable = Observable.FromEventPattern<EventHandler, EventArgs>(
			//	@this => _processFormViewModel.OnCancel += @this,
			//	@this => _processFormViewModel.OnCancel -= @this)
			//	.Subscribe((e) =>
			//	{
			//		ProcessFormViewModel.Data = null;
			//	});

			//_saveOperationReactiveCommand = ReactiveCommand.Create(() =>
			//{
			//	if (ProcessFormViewModel.Data.Id.Equals(Guid.Empty))
			//	{

			//	}
			//	else
			//	{

			//	}
			//});

			_addProcessReactiveCommand = ReactiveCommand.Create(() =>
			{
				FormData = Mapper.Map<IProcessViewModel>(new ProcessEntity());
			});

			_cancelOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				FormData = null;
			});

			LoadOptionsCommandSetup();

			this.WhenAnyValue(viewModel => viewModel.FormData).Where(option => option != null).Subscribe(option =>
			{
				FormData = option;
			});
		}

		/// <summary>
		/// Gets or sets the processes loading status.
		/// </summary>
		public bool IsLoadingProcesses
		{
			get => _isLoadingProcesses;
			set => this.RaiseAndSetIfChanged(ref _isLoadingProcesses, value);
		}

		/// <summary>
		/// Gets the load options command instance.
		/// </summary>
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesReactiveCommand => _loadOptionsReactiveCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<IProcessViewModel> ProcessesReactiveDerivedList => _processesReactiveDerivedList;

		/// <summary>
		/// Gets the form instance.
		/// </summary>
		public IProcessViewModel FormData
		{
			get => _formData;
			set => this.RaiseAndSetIfChanged(ref _formData, value);
		}

		/// <summary>
		/// Gets the add process command instance.
		/// </summary>
		public ReactiveCommand AddProcessReactiveCommand => _addProcessReactiveCommand;

		public ReactiveCommand CancelOperationReactiveCommand => _cancelOperationReactiveCommand;

		public ReactiveCommand SaveOperationReactiveCommand => _saveOperationReactiveCommand;

		/// <summary>
		/// Setup the load options comand actions and observables.
		/// </summary>
		private void LoadOptionsCommandSetup()
		{
			_loadOptionsReactiveCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _processesRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsReactiveCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingProcesses);

			_loadOptionsReactiveCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadOptionsReactiveCommand.Subscribe(options =>
			{
				_entities.Clear();
				_entities.AddRange(options);
			});
		}
	}
}
