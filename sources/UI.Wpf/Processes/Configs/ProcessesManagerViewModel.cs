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
	//
	public interface IProcessesManagerViewModel
	{
		bool IsPopupOpen { get; }
		string FilterText { get; set; }
		bool IsLoadingProcesses { get; }
		ReactiveCommand AddProcessCommand { get; }
		ReactiveCommand SaveProcessCommand { get; }
		ReactiveCommand CancelFormCommand { get; }
		ReactiveCommand DeleteProcessCommand { get; }
		ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadProcessesCommand { get; }
		IReactiveDerivedList<IProcessViewModel> Processes { get; }
		IValidator<IProcessViewModel> FormDataValidator { get; }
		IProcessViewModel SelectedProcess { get; set; }
		IProcessViewModel FormData { get; set; }
	}

	//
	public class ProcessesManagerViewModel : ReactiveObject, IProcessesManagerViewModel
	{
		private readonly IProcessRepository _processesRepository;
		private readonly IReactiveList<ProcessEntity> _processesSource;

		private string _filterText;
		private bool _isPopupOpen;
		private bool _isLoadingProcesses;
		private ReactiveCommand _addProcessCommand;
		private ReactiveCommand _saveProcessCommand;
		private ReactiveCommand _cancelFormCommand;
		private ReactiveCommand _deleteProcessCommand;
		private ReactiveCommand<Unit, IEnumerable<ProcessEntity>> _loadProcessesCommand;
		private IReactiveDerivedList<IProcessViewModel> _processesList;
		private IValidator<IProcessViewModel> _formDataValidator;
		private IProcessViewModel _selectedProcess;
		private IProcessViewModel _formData;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerViewModel(IProcessRepository processesRepository = null, IValidator<IProcessViewModel> processViewModelaValidator = null)
		{
			_processesRepository = processesRepository ?? Locator.CurrentMutable.GetService<IProcessRepository>();
			_formDataValidator = processViewModelaValidator ?? Locator.CurrentMutable.GetService<IValidator<IProcessViewModel>>();

			// Lists
			_processesSource = new ReactiveList<ProcessEntity>() { ChangeTrackingEnabled = true };
			_processesList = _processesSource.CreateDerivedCollection(
				selector: entity => Mapper.Map<IProcessViewModel>(entity),
				filter: entity => FilterEntity(entity),
				signalReset: this.ObservableForProperty(@this => @this.FilterText).Throttle(TimeSpan.FromMilliseconds(175), RxApp.MainThreadScheduler)
			);

			// Add
			_addProcessCommand = ReactiveCommand.Create(() => AddProcessCommandAction());

			// Edit
			this.WhenAnyValue(viewModel => viewModel.SelectedProcess)
				.Where(option => option != null)
				.Subscribe(process => EditProcessCommandAction(process));

			// Save
			_saveProcessCommand = ReactiveCommand.Create(() => SaveProcessAction());

			// Cancel
			_cancelFormCommand = ReactiveCommand.Create(() => CancelFormCommandAction());

			// Delete
			_deleteProcessCommand = ReactiveCommand.Create(() => DeleteProcessCommandAction());

			// Load Processess
			_loadProcessesCommand = ReactiveCommand.CreateFromTask(async () => await LoadProcessesCommandAction());
			_loadProcessesCommand.IsExecuting.BindTo(this, @this => @this.IsLoadingProcesses);
			_loadProcessesCommand.Subscribe(entities => LoadProcessesCommandHandler(entities));
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
		public IReactiveDerivedList<IProcessViewModel> Processes => _processesList;

		public ReactiveCommand AddProcessCommand => _addProcessCommand;
		public ReactiveCommand SaveProcessCommand => _saveProcessCommand;
		public ReactiveCommand CancelFormCommand => _cancelFormCommand;
		public ReactiveCommand DeleteProcessCommand => _deleteProcessCommand;
		public ReactiveCommand<Unit, IEnumerable<ProcessEntity>> LoadProcessesCommand => _loadProcessesCommand;

		private bool FilterEntity(ProcessEntity entity)
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
		}

		private void AddProcessCommandAction()
		{
			FormData = Mapper.Map<IProcessViewModel>(new ProcessEntity());
		}

		private void EditProcessCommandAction(IProcessViewModel process)
		{
			FormData = Mapper.Map<IProcessViewModel>(process);
		}

		private void SaveProcessAction()
		{
			var validationResult = _formDataValidator.Validate(FormData);

			FormData.SetErrors(validationResult.Errors);

			if (FormData.IsValid)
			{
				var entity = Mapper.Map<ProcessEntity>(FormData);

				if (FormData.Id.Equals(Guid.Empty))
				{
					entity = _processesRepository.Add(entity);
					_processesSource.Add(entity);
				}
				else
				{
					_processesRepository.Update(entity);
					var currentEntity = _processesSource.FirstOrDefault(n => n.Id == entity.Id);
					_processesSource.Remove(currentEntity);
					_processesSource.Add(entity);
				}

				FormData = null;
			}
		}

		private void CancelFormCommandAction()
		{
			SelectedProcess = null;
			FormData = null;
		}

		private void DeleteProcessCommandAction()
		{
			var deleteId = FormData.Id;
			_processesRepository.Delete(deleteId);
			var currentEnttiy = _processesSource.Where(o => o.Id == deleteId).SingleOrDefault();
			if (currentEnttiy != null)
			{
				_processesSource.Remove(currentEnttiy);
			}
			FormData = null;
			IsPopupOpen = false;
		}

		private Task<IEnumerable<ProcessEntity>> LoadProcessesCommandAction() => Task.Run(() =>
		{
			var items = _processesRepository.GetAll();
			return Task.FromResult(items);
		});

		private void LoadProcessesCommandHandler(IEnumerable<ProcessEntity> entities)
		{
			_processesSource.Clear();
			_processesSource.AddRange(entities);
		}
	}
}
