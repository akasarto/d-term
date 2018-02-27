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
		bool IsPopupOpen { get; }
		string FilterText { get; set; }
		bool IsLoadingProcesses { get; }
		ReactiveCommand AddProcessReactiveCommand { get; }
		ReactiveCommand SaveOperationReactiveCommand { get; }
		ReactiveCommand CancelOperationReactiveCommand { get; }
		ReactiveCommand DeleteOperationReactiveCommand { get; }
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
		private IProcessViewModel _formData;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerViewModel(IProcessesRepository processesRepository = null)
		{
			var locator = Locator.CurrentMutable;

			_processesRepository = processesRepository ?? locator.GetService<IProcessesRepository>();

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
				FormData = Mapper.Map<IProcessViewModel>(new ProcessEntity());
			});

			/*
			 * Edit
			 */
			this.WhenAnyValue(viewModel => viewModel.FormData).Where(option => option != null).Subscribe(option =>
			{
				FormData = option;
			});

			/*
			 * Save
			 */
			_saveOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				if (FormData.Id.Equals(Guid.Empty))
				{
					System.Windows.MessageBox.Show("Adding");
				}
				else
				{
					System.Windows.MessageBox.Show("Editing");
				}
			});

			/*
			 * Cancel
			 */
			_cancelOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				FormData = null;
			});

			/*
			 * Delete
			 */
			_deleteOperationReactiveCommand = ReactiveCommand.Create(() =>
			{
				System.Windows.MessageBox.Show("Deleting");
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

		public ReactiveCommand SaveOperationReactiveCommand => _saveOperationReactiveCommand;

		public ReactiveCommand CancelOperationReactiveCommand => _cancelOperationReactiveCommand;

		public ReactiveCommand DeleteOperationReactiveCommand => _deleteOperationReactiveCommand;
	}
}
