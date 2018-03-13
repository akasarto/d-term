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
	public interface IConsolesTabViewModel
	{
		bool IsPopupOpen { get; }
		string FilterText { get; set; }
		bool IsLoadingProcesses { get; }
		ReactiveCommand AddProcessCommand { get; }
		ReactiveCommand SaveProcessCommand { get; }
		ReactiveCommand CancelFormCommand { get; }
		ReactiveCommand DeleteProcessCommand { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesCommand { get; }
		IReactiveDerivedList<IProcessViewModel> Processes { get; }
		IValidator<IProcessViewModel> FormDataValidator { get; }
		IProcessViewModel SelectedProcess{ get; set; }
		IProcessViewModel FormData { get; set; }
	}

	public class ConsolesTabViewModel : ReactiveObject, IConsolesTabViewModel
	{
		//
		private readonly IProcessRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _entities;

		//
		private string _filterText;
		private bool _isPopupOpen;
		private bool _isLoadingProcesses;
		private ReactiveCommand _addProcessCommand;
		private ReactiveCommand _saveProcessCommand;
		private ReactiveCommand _cancelFormCommand;
		private ReactiveCommand _deleteProcessCommand;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadProcessesCommand;
		private IReactiveDerivedList<IProcessViewModel> _processes;
		private IValidator<IProcessViewModel> _formDataValidator;
		private IProcessViewModel _selectedProcess;
		private IProcessViewModel _formData;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesTabViewModel(IProcessRepository processesRepository = null, IValidator<IProcessViewModel> processViewModelaValidator = null)
		{
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();
			_formDataValidator = processViewModelaValidator ?? Locator.CurrentMutable.GetService<IValidator<IProcessViewModel>>();

			_entities = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = true };

			_processes = _entities.CreateDerivedCollection(
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
			_addProcessCommand = ReactiveCommand.Create(() =>
			{
				FormData = Mapper.Map<IProcessViewModel>(new ProcessEntity());
			});

			/*
			 * Edit
			 */
			this.WhenAnyValue(viewModel => viewModel.SelectedProcess).Where(option => option != null).Subscribe(process =>
			{
				FormData = Mapper.Map<IProcessViewModel>(process);
			});

			/*
			 * Save
			 */
			_saveProcessCommand = ReactiveCommand.Create(() =>
			{
				var validationResult = _formDataValidator.Validate(FormData);

				FormData.SetErrors(validationResult.Errors);

				if (FormData.IsValid)
				{
					var entity = Mapper.Map<ProcessEntity>(FormData);

					if (FormData.Id.Equals(Guid.Empty))
					{
						entity = _processesRepository.Add(entity);
						_entities.Add(entity);
					}
					else
					{
						_processesRepository.Update(entity);
						var currentEntity = _entities.FirstOrDefault(n => n.Id == entity.Id);
						_entities.Remove(currentEntity);
						_entities.Add(entity);
					}

					FormData = null;
				}
			});

			/*
			 * Cancel
			 */
			_cancelFormCommand = ReactiveCommand.Create(() =>
			{
				SelectedProcess = null;
				FormData = null;
			});

			/*
			 * Delete
			 */
			_deleteProcessCommand = ReactiveCommand.Create(() =>
			{
				var deleteId = FormData.Id;
				_processesRepository.Delete(deleteId);
				var currentEnttiy = _entities.Where(o => o.Id == deleteId).SingleOrDefault();
				if (currentEnttiy != null)
				{
					_entities.Remove(currentEnttiy);
				}
				FormData = null;
				IsPopupOpen = false;
			});

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
			});

			_loadProcessesCommand.Subscribe(options =>
			{
				_entities.Clear();
				_entities.AddRange(options);
			});
		}

		public string FilterText
		{
			get => _filterText;
			set => this.RaiseAndSetIfChanged(ref _filterText, value);
		}

		public bool IsPopupOpen
		{
			get => _isPopupOpen;
			set => this.RaiseAndSetIfChanged(ref _isPopupOpen, value);
		}

		public bool IsLoadingProcesses
		{
			get => _isLoadingProcesses;
			set => this.RaiseAndSetIfChanged(ref _isLoadingProcesses, value);
		}

		public IProcessViewModel SelectedProcess
		{
			get => _selectedProcess;
			set => this.RaiseAndSetIfChanged(ref _selectedProcess, value);
		}

		public IProcessViewModel FormData
		{
			get => _formData;
			set => this.RaiseAndSetIfChanged(ref _formData, value);
		}

		public IValidator<IProcessViewModel> FormDataValidator => _formDataValidator;
		public IReactiveDerivedList<IProcessViewModel> Processes => _processes;

		public ReactiveCommand AddProcessCommand => _addProcessCommand;
		public ReactiveCommand SaveProcessCommand => _saveProcessCommand;
		public ReactiveCommand CancelFormCommand => _cancelFormCommand;
		public ReactiveCommand DeleteProcessCommand => _deleteProcessCommand;
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesCommand => _loadProcessesCommand;
	}
}
