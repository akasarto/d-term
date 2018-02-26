using AutoMapper;
using Consoles.Core;
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
	/// Console configurations view model interface.
	/// </summary>
	public interface IConsoleOptionsManagerViewModel
	{
		bool IsBusy { get; }
		ReactiveCommand AddOptionCommand { get; }
		ReactiveCommand<Unit, List<ConsoleOption>> LoadOptionsCommand { get; }
		IReactiveDerivedList<IConsoleOptionFormViewModel> Options { get; }
		IConsoleOptionFormViewModel FormData { get; set; }
	}

	/// <summary>
	/// App console configurations view model implementation.
	/// </summary>
	public class ConsoleOptionsManagerViewModel : ReactiveObject, IConsoleOptionsManagerViewModel
	{
		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository;
		private readonly IReactiveList<ConsoleOption> _consoleOptionsSourceList;

		//
		private bool _isBusy;
		private ReactiveCommand _addOptionCommand;
		private ReactiveCommand _saveOptionCommand;
		private ReactiveCommand _cancelOptionCommand;
		private ReactiveCommand<Unit, List<ConsoleOption>> _loadOptionsCommand;
		private IReactiveDerivedList<IConsoleOptionFormViewModel> _options;
		private IConsoleOptionFormViewModel _formData;
		private IDisposable _deleteFormEvent;
		private IDisposable _cancelFormEvent;
		private IDisposable _saveFormEvent;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsManagerViewModel(IConsoleOptionsRepository consoleOptionsRepository = null)
		{
			var locator = Locator.CurrentMutable;

			_consoleOptionsRepository = consoleOptionsRepository ?? locator.GetService<IConsoleOptionsRepository>();

			_consoleOptionsSourceList = new ReactiveList<ConsoleOption>();

			_options = _consoleOptionsSourceList.CreateDerivedCollection(
				selector: option => Mapper.Map<IConsoleOptionFormViewModel>(option)
			);

			_addOptionCommand = ReactiveCommand.Create(() =>
			{
				FormData = locator.GetService<IConsoleOptionFormViewModel>();
			});

			_saveOptionCommand = ReactiveCommand.Create(() =>
			{
				if (FormData.Id.Equals(Guid.Empty))
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

			this.WhenAnyValue(@this => @this.FormData).Subscribe(data =>
			{
				if (data != null)
				{
					_cancelFormEvent = Observable.FromEventPattern<EventHandler, EventArgs>(
						@this => FormData.OnCancel += @this,
						@this => FormData.OnCancel -= @this)
						.Subscribe((e) =>
						{
							FormData = null;
						});
				}
				else
				{
					_cancelFormEvent?.Dispose();
				}
			});

			this.WhenAnyValue(viewModel => viewModel.FormData).Where(option => option != null).Subscribe(option =>
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
		public ReactiveCommand<Unit, List<ConsoleOption>> LoadOptionsCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<IConsoleOptionFormViewModel> Options => _options;

		/// <summary>
		/// Gets or sets the current option being added/edited.
		/// </summary>
		public IConsoleOptionFormViewModel FormData
		{
			get => _formData;
			set => this.RaiseAndSetIfChanged(ref _formData, value);
		}

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
