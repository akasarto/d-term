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
	public interface IConsoleConfigsViewModel
	{
		bool IsBusy { get; }
		ReactiveCommand AddOptionCommand { get; }
		ReactiveCommand<Unit, List<ConsoleEntity>> LoadOptionsCommand { get; }
		IReactiveDerivedList<IConsoleViewModel> Options { get; }
		IConsoleFormViewModel Form { get; set; }
	}

	/// <summary>
	/// App console configurations view model implementation.
	/// </summary>
	public class ConsoleConfigsViewModel : ReactiveObject, IConsoleConfigsViewModel
	{
		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository;
		private readonly IReactiveList<ConsoleEntity> _consoleOptionsSourceList;

		//
		private bool _isBusy;
		private IDisposable _saveFormEvent;
		private IDisposable _deleteFormEvent;
		private IDisposable _cancelFormEvent;
		private IConsoleFormViewModel _form;
		private ReactiveCommand _addOptionCommand;
		private ReactiveCommand _saveOptionCommand;
		private ReactiveCommand _cancelOptionCommand;
		private ReactiveCommand<Unit, List<ConsoleEntity>> _loadOptionsCommand;
		private IReactiveDerivedList<IConsoleViewModel> _options;


		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleConfigsViewModel(IConsoleOptionsRepository consoleOptionsRepository = null)
		{
			var locator = Locator.CurrentMutable;

			_consoleOptionsRepository = consoleOptionsRepository ?? locator.GetService<IConsoleOptionsRepository>();

			_consoleOptionsSourceList = new ReactiveList<ConsoleEntity>();

			_options = _consoleOptionsSourceList.CreateDerivedCollection(
				selector: option => Mapper.Map<IConsoleViewModel>(option)
			);

			_addOptionCommand = ReactiveCommand.Create(() =>
			{
				Form = locator.GetService<IConsoleFormViewModel>();
			});

			_saveOptionCommand = ReactiveCommand.Create(() =>
			{
				//if (FormData.Id.Equals(Guid.Empty))
				//{

				//}
				//else
				//{

				//}
			});

			_cancelOptionCommand = ReactiveCommand.Create(() =>
			{

			});

			LoadOptionsCommandSetup();

			this.WhenAnyValue(@this => @this.Form).Subscribe(data =>
			{
				if (data != null)
				{
					_cancelFormEvent = Observable.FromEventPattern<EventHandler, EventArgs>(
						@this => Form.OnCancel += @this,
						@this => Form.OnCancel -= @this)
						.Subscribe((e) =>
						{
							Form = null;
						});
				}
				else
				{
					_cancelFormEvent?.Dispose();
				}
			});

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
		public ReactiveCommand<Unit, List<ConsoleEntity>> LoadOptionsCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<IConsoleViewModel> Options => _options;

		/// <summary>
		/// Gets or sets the form instance.
		/// </summary>
		public IConsoleFormViewModel Form
		{
			get => _form;
			set => this.RaiseAndSetIfChanged(ref _form, value);
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
