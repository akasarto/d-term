using AutoMapper;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Consoles manager view model interface.
	/// </summary>
	public interface IConsolesManagerViewModel
	{
		bool IsBusy { get; }
		ReactiveCommand AddOptionCommand { get; }
		ReactiveCommand<Unit, List<ProcessEntity>> LoadOptionsCommand { get; }
		IReactiveDerivedList<IConsoleViewModel> Options { get; }
		IConsoleFormViewModel Form { get; }
	}

	/// <summary>
	/// App consoles manager view model implementation.
	/// </summary>
	public class ConsolesManagerViewModel : ReactiveObject, IConsolesManagerViewModel
	{
		//
		private readonly IReactiveList<ProcessEntity> _consoleOptionsSourceList;
		private readonly IProcessesRepository _consoleOptionsRepository;
		private readonly IConsoleFormViewModel _consoleFormViewModel;

		//
		private bool _isBusy;
		private IDisposable _saveFormEvent;
		private IDisposable _deleteFormEvent;
		private IDisposable _cancelFormEvent;
		private ReactiveCommand _addOptionCommand;
		private ReactiveCommand _saveOptionCommand;
		private ReactiveCommand _cancelOptionCommand;
		private ReactiveCommand<Unit, List<ProcessEntity>> _loadOptionsCommand;
		private IReactiveDerivedList<IConsoleViewModel> _options;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesManagerViewModel(IProcessesRepository consoleOptionsRepository = null, IConsoleFormViewModel consoleFormViewModel = null)
		{
			var locator = Locator.CurrentMutable;

			_consoleOptionsRepository = consoleOptionsRepository ?? locator.GetService<IProcessesRepository>();
			_consoleFormViewModel = consoleFormViewModel ?? locator.GetService<IConsoleFormViewModel>();

			_consoleOptionsSourceList = new ReactiveList<ProcessEntity>();

			_options = _consoleOptionsSourceList.CreateDerivedCollection(
				selector: option => Mapper.Map<IConsoleViewModel>(option)
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
				Form.Data = locator.GetService<IConsoleViewModel>();
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
		public IReactiveDerivedList<IConsoleViewModel> Options => _options;

		/// <summary>
		/// Gets the form instance.
		/// </summary>
		public IConsoleFormViewModel Form => _consoleFormViewModel;

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
