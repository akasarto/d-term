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
		ReactiveCommand AddOptionCommand { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand { get; }
		IReactiveDerivedList<IProcessViewModel> Options { get; }
		IProcessFormViewModel Form { get; }
	}

	/// <summary>
	/// App processes manager view model implementation.
	/// </summary>
	public class ProcessesManagerViewModel : ReactiveObject, IProcessesManagerViewModel
	{
		//
		private readonly IReactiveList<ProcessEntity> _consoleOptionsSourceList;
		private readonly IProcessesRepository _consoleOptionsRepository;
		private readonly IProcessFormViewModel _consoleFormViewModel;

		//
		private bool _isBusy;
		private IDisposable _saveFormEvent;
		private IDisposable _deleteFormEvent;
		private IDisposable _cancelFormEvent;
		private ReactiveCommand _addOptionCommand;
		private ReactiveCommand _saveOptionCommand;
		private ReactiveCommand _cancelOptionCommand;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private IReactiveDerivedList<IProcessViewModel> _options;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerViewModel(IProcessesRepository consoleOptionsRepository = null, IProcessFormViewModel consoleFormViewModel = null)
		{
			var locator = Locator.CurrentMutable;

			_consoleOptionsRepository = consoleOptionsRepository ?? locator.GetService<IProcessesRepository>();
			_consoleFormViewModel = consoleFormViewModel ?? locator.GetService<IProcessFormViewModel>();

			_consoleOptionsSourceList = new ReactiveList<ProcessEntity>();

			_options = _consoleOptionsSourceList.CreateDerivedCollection(
				selector: option => Mapper.Map<IProcessViewModel>(option)
			);

			_cancelFormEvent = Observable.FromEventPattern<EventHandler, EventArgs>(
				@this => _consoleFormViewModel.OnCancel += @this,
				@this => _consoleFormViewModel.OnCancel -= @this)
				.Subscribe((e) =>
				{
					Form.Data = null;
				});

			_addOptionCommand = ReactiveCommand.Create(() =>
			{
				Form.Data = locator.GetService<IProcessViewModel>();
			});

			_saveOptionCommand = ReactiveCommand.Create(() =>
			{
				if (Form.Data.Id.Equals(Guid.Empty))
				{

				}
				else
				{

				}
			});

			_cancelOptionCommand = ReactiveCommand.Create(() =>
			{

			});

			LoadOptionsCommandSetup();

			this.WhenAnyValue(viewModel => viewModel.Form).Where(option => option != null).Subscribe(option =>
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
		public ReactiveCommand AddOptionCommand => _addOptionCommand;

		/// <summary>
		/// Gets the load options command instance.
		/// </summary>
		public ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<IProcessViewModel> Options => _options;

		/// <summary>
		/// Gets the form instance.
		/// </summary>
		public IProcessFormViewModel Form => _consoleFormViewModel;

		/// <summary>
		/// Setup the load options comand actions and observables.
		/// </summary>
		private void LoadOptionsCommandSetup()
		{
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _consoleOptionsRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsBusy);

			_loadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadOptionsCommand.Subscribe(options =>
			{
				_consoleOptionsSourceList.Clear();
				_consoleOptionsSourceList.AddRange(options);
			});
		}
	}
}
