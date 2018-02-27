using AutoMapper;
using FluentValidation;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
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
		bool IsPopupOpen { get; }
		string FilterText { get; set; }
		bool IsLoadingProcesses { get; }
		ReactiveCommand AddProcessReactiveCommand { get; }
		ReactiveCommand SaveOperationReactiveCommand { get; }
		ReactiveCommand CancelOperationReactiveCommand { get; }
		ReactiveCommand DeleteOperationReactiveCommand { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesReactiveCommand { get; }
		IReactiveDerivedList<IProcessViewModel> ProcessesReactiveDerivedList { get; }
		IValidator<IProcessViewModel> ProcessViewModelaValidator { get; }
		IProcessViewModel ProcessViewModel { get; set; }
	}

	/// <summary>
	/// App processes manager view model implementation.
	/// </summary>
	public class ProcessesManagerViewModel : ReactiveObject, IProcessesManagerViewModel
	{
		//
		private readonly IProcessesRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _entitiesReactiveList;

		//
		private string _filterText;
		private bool _isPopupOpen;
		private bool _isLoadingProcesses;
		private ReactiveCommand _addProcessReactiveCommand;
		private ReactiveCommand _saveOperationReactiveCommand;
		private ReactiveCommand _cancelOperationReactiveCommand;
		private ReactiveCommand _deleteOperationReactiveCommand;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsReactiveCommand;
		private IReactiveDerivedList<IProcessViewModel> _processesReactiveDerivedList;
		private IValidator<IProcessViewModel> _processViewModelaValidator;
		private IProcessViewModel _processViewModel;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerViewModel(IProcessesRepository processesRepository = null, IValidator<IProcessViewModel> processViewModelaValidator = null)
		{
			var locator = Locator.CurrentMutable;

			_processesRepository = processesRepository ?? locator.GetService<IProcessesRepository>();
			_processViewModelaValidator = processViewModelaValidator ?? locator.GetService<IValidator<IProcessViewModel>>();

			_entitiesReactiveList = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = true };

			_processesReactiveDerivedList = _entitiesReactiveList.CreateDerivedCollection(
				selector: entity => Mapper.Map<IProcessViewModel>(entity),
				filter: entity =>
				{
					var filterText = FilterText;

					if (!string.IsNullOrEmpty(filterText))
					{
						var nameMatch = (entity?.Name?.ToLower() ?? string.Empty).Contains(filterText.ToLower());
						var processBasePathMatch = (entity?.ProcessBasePath.ToString()?.ToLower() ?? string.Empty).Contains(filterText.ToLower());
						var processExecutableNameMatch = (entity?.ProcessExecutableName?.ToLower() ?? string.Empty).Contains(filterText.ToLower());
						var processStartupArgsMatch = (entity?.ProcessStartupArgs?.ToLower() ?? string.Empty).Contains(filterText.ToLower());

						return nameMatch || processBasePathMatch || processExecutableNameMatch || processStartupArgsMatch;
					}

					return true;
				},
				signalReset: this.ObservableForProperty(@this => @this.FilterText).Throttle(TimeSpan.FromMilliseconds(175), RxApp.MainThreadScheduler)
			);

			/*
			 * Add
			 */
			_addProcessReactiveCommand = ReactiveCommand.Create(() =>
			{
				ProcessViewModel = Mapper.Map<IProcessViewModel>(new ProcessEntity());
			});

			/*
			 * Edit
			 */
			this.WhenAnyValue(viewModel => viewModel.ProcessViewModel).Where(option => option != null).Subscribe(option =>
			{
				ProcessViewModel = option;
			});

			/*
			 * Save
			 */
			_saveOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				var validationResult = _processViewModelaValidator.Validate(ProcessViewModel);

				ProcessViewModel.SetErrors(validationResult.Errors);

				if (ProcessViewModel.IsValid)
				{
					var entity = Mapper.Map<ProcessEntity>(ProcessViewModel);

					if (ProcessViewModel.Id.Equals(Guid.Empty))
					{
						entity = _processesRepository.Add(entity);
						_entitiesReactiveList.Add(entity);
					}
					else
					{
						_processesRepository.Update(entity);
						var currentEntity = _entitiesReactiveList.FirstOrDefault(n => n.Id == entity.Id);
						_entitiesReactiveList.Remove(currentEntity);
						_entitiesReactiveList.Add(entity);
					}

					ProcessViewModel = null;
				}
			});

			/*
			 * Cancel
			 */
			_cancelOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				ProcessViewModel = null;
			});

			/*
			 * Delete
			 */
			_deleteOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				var deleteId = ProcessViewModel.Id;
				_processesRepository.Delete(deleteId);
				var currentEnttiy = _entitiesReactiveList.Where(o => o.Id == deleteId).SingleOrDefault();
				if (currentEnttiy != null)
				{
					_entitiesReactiveList.Remove(currentEnttiy);
				}
				ProcessViewModel = null;
				IsPopupOpen = false;
			});

			/*
			 * Load Options
			 */
			_loadOptionsReactiveCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _processesRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsReactiveCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingProcesses);

			_loadOptionsReactiveCommand.ThrownExceptions.Subscribe(@exception =>
			{
			});

			_loadOptionsReactiveCommand.Subscribe(options =>
			{
				_entitiesReactiveList.Clear();
				_entitiesReactiveList.AddRange(options);
			});
		}

		/// <summary>
		/// Tracks whether the delete popup is open or not.
		/// </summary>
		public bool IsPopupOpen
		{
			get => _isPopupOpen;
			set => this.RaiseAndSetIfChanged(ref _isPopupOpen, value);
		}

		/// <summary>
		/// Gets or sets the text used to filter processes.
		/// </summary>
		public string FilterText
		{
			get => _filterText;
			set => this.RaiseAndSetIfChanged(ref _filterText, value);
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
		/// Gets the form data validator instance.
		/// </summary>
		public IValidator<IProcessViewModel> ProcessViewModelaValidator => _processViewModelaValidator;

		/// <summary>
		/// Gets or sets the process instance to add/edit.
		/// </summary>
		public IProcessViewModel ProcessViewModel
		{
			get => _processViewModel;
			set => this.RaiseAndSetIfChanged(ref _processViewModel, value);
		}

		/// <summary>
		/// Gets the add process command instance.
		/// </summary>
		public ReactiveCommand AddProcessReactiveCommand => _addProcessReactiveCommand;

		/// <summary>
		/// Gets the add/edit operation save command instance.
		/// </summary>
		public ReactiveCommand SaveOperationReactiveCommand => _saveOperationReactiveCommand;

		/// <summary>
		/// Gets the add/edit operation cancel command instance.
		/// </summary>
		public ReactiveCommand CancelOperationReactiveCommand => _cancelOperationReactiveCommand;

		/// <summary>
		/// Gets the edit operation delete command instance.
		/// </summary>
		public ReactiveCommand DeleteOperationReactiveCommand => _deleteOperationReactiveCommand;
	}
}
