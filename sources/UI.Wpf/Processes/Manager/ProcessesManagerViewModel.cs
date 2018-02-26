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
		bool IsBusy { get; }
		ReactiveCommand AddProcessReactiveCommand { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadProcessesReactiveCommand { get; }
		IReactiveDerivedList<IProcessViewModel> ProcessesReactiveDerivedList { get; }
		IProcessFormViewModel ProcessFormViewModel { get; }
	}

	/// <summary>
	/// App processes manager view model implementation.
	/// </summary>
	public class ProcessesManagerViewModel : ReactiveObject, IProcessesManagerViewModel
	{
		//
		private readonly IReactiveList<ProcessEntity> _entities;
		private readonly IProcessesRepository _processesRepository;
		private readonly IProcessFormViewModel _processFormViewModel;

		//
		private bool _isBusy;
		private IDisposable _onCancelEventDisposable;
		private ReactiveCommand _addOptionReactiveCommand;
		private ReactiveCommand _saveOptionReactiveCommand;
		private ReactiveCommand _cancelOptionReactiveCommand;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsReactiveCommand;
		private IReactiveDerivedList<IProcessViewModel> _processesReactiveDerivedList;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerViewModel(IProcessesRepository processesRepository = null, IProcessFormViewModel processFormViewModel = null)
		{
			var locator = Locator.CurrentMutable;

			_processesRepository = processesRepository ?? locator.GetService<IProcessesRepository>();
			_processFormViewModel = processFormViewModel ?? locator.GetService<IProcessFormViewModel>();

			_entities = new ReactiveList<ProcessEntity>();

			_processesReactiveDerivedList = _entities.CreateDerivedCollection(
				selector: option => Mapper.Map<IProcessViewModel>(option)
			);

			_onCancelEventDisposable = Observable.FromEventPattern<EventHandler, EventArgs>(
				@this => _processFormViewModel.OnCancel += @this,
				@this => _processFormViewModel.OnCancel -= @this)
				.Subscribe((e) =>
				{
					ProcessFormViewModel.Data = null;
				});

			_addOptionReactiveCommand = ReactiveCommand.Create(() =>
			{
				ProcessFormViewModel.Data = locator.GetService<IProcessViewModel>();
			});

			_saveOptionReactiveCommand = ReactiveCommand.Create(() =>
			{
				if (ProcessFormViewModel.Data.Id.Equals(Guid.Empty))
				{

				}
				else
				{

				}
			});

			_cancelOptionReactiveCommand = ReactiveCommand.Create(() =>
			{

			});

			LoadOptionsCommandSetup();

			this.WhenAnyValue(viewModel => viewModel.ProcessFormViewModel).Where(option => option != null).Subscribe(option =>
			{
				//FormData = Mapper.Map<IConsoleOptionFormViewModel>(option);
			});
		}

		/// <summary>
		/// Gets or sets the loading status.
		/// </summary>
		public bool IsBusy
		{
			get => _isBusy;
			set => this.RaiseAndSetIfChanged(ref _isBusy, value);
		}

		/// <summary>
		/// Gets the add console option command instance.
		/// </summary>
		public ReactiveCommand AddProcessReactiveCommand => _addOptionReactiveCommand;

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
		public IProcessFormViewModel ProcessFormViewModel => _processFormViewModel;

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

			_loadOptionsReactiveCommand.IsExecuting.BindTo(this, @this => @this.IsBusy);

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
