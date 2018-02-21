using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsoleSettingsDialogViewModel : BaseViewModel
	{
		private bool _isManaging;
		private ConsoleOptionFormViewModel _formData;
		private ReactiveList<ConsoleOption> _consoleOptions;
		private IReactiveDerivedList<ConsoleOptionViewModel> _consoleOptionViewModels;
		private ConsoleOptionViewModel _selectedOption;

		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleSettingsDialogViewModel(IConsoleOptionsRepository consoleOptionsRepository)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? throw new ArgumentNullException(nameof(consoleOptionsRepository), nameof(ConsoleSettingsDialogViewModel));

			_consoleOptions = new ReactiveList<ConsoleOption>();

			ConsoleOptions = _consoleOptions.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: noteEntity => Mapper.Map<ConsoleOptionViewModel>(noteEntity),
				orderer: (noteX, noteY) => noteX.OrderIndex.CompareTo(noteY.OrderIndex),
				scheduler: RxApp.MainThreadScheduler
			);

			_consoleOptions.ItemsAdded.Subscribe(added =>
			{

			});

			_consoleOptions.ItemsRemoved.Subscribe(removed =>
			{

			});

			this.ObservableForProperty(viewModel => viewModel.SelectedOption).Where(option => option.Value != null).Subscribe(option =>
			{
				FormData = Mapper.Map<ConsoleOptionFormViewModel>(option.Value);
				IsManaging = true;
			});

			SetupCommands();
		}

		/// <summary>
		/// Add a new console option.
		/// </summary>
		public ReactiveCommand AddCommand { get; protected set; }

		/// <summary>
		/// Cancel the add/edit operation.
		/// </summary>
		public ReactiveCommand CancelCommand { get; protected set; }

		/// <summary>
		/// Delete a console option.
		/// </summary>
		public ReactiveCommand DeleteCommand { get; protected set; }

		/// <summary>
		/// Save the changes of an add/edit operation.
		/// </summary>
		public ReactiveCommand SaveCommand { get; protected set; }

		/// <summary>
		/// Get the current console options list.
		/// </summary>
		public IReactiveDerivedList<ConsoleOptionViewModel> ConsoleOptions
		{
			get => _consoleOptionViewModels;
			set => this.RaiseAndSetIfChanged(ref _consoleOptionViewModels, value);
		}

		/// <summary>
		/// Gets or sets the current selected option.
		/// </summary>
		public ConsoleOptionViewModel SelectedOption
		{
			get => _selectedOption;
			set => this.RaiseAndSetIfChanged(ref _selectedOption, value);
		}

		/// <summary>
		/// Gets or sets the current option being added/edited.
		/// </summary>
		public ConsoleOptionFormViewModel FormData
		{
			get => _formData;
			set => this.RaiseAndSetIfChanged(ref _formData, value);
		}

		/// <summary>
		/// Gets or sets whether and option is being added/edited.
		/// </summary>
		public bool IsManaging
		{
			get => _isManaging;
			set => this.RaiseAndSetIfChanged(ref _isManaging, value);
		}

		/// <summary>
		/// Initialize the model.
		/// </summary>
		public void Initialize()
		{
			Observable.Start(
				() => _consoleOptionsRepository.GetAll()
			).Subscribe(
				options => _consoleOptions.AddRange(options)
			);
		}

		private void SetupCommands()
		{
			AddCommand = ReactiveCommand.Create(() =>
			{
				FormData = new ConsoleOptionFormViewModel();
				IsManaging = true;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				SelectedOption = null;
				IsManaging = false;
			});

			DeleteCommand = ReactiveCommand.Create(() =>
			{
				_consoleOptionsRepository.Delete(FormData.Id);
				var deletedOption = _consoleOptions.Where(o => o.Id == FormData.Id).SingleOrDefault();
				if (deletedOption != null)
				{
					_consoleOptions.Remove(deletedOption);
					MessageBus.Current.SendMessage(new ConsoleOptionDeletedMessage(deletedOption));
				}
				SelectedOption = null;
				IsManaging = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				FormData.Validate();

				if (FormData.IsValid)
				{
					var consoleOption = Mapper.Map<ConsoleOption>(FormData);

					if (consoleOption.Id == Guid.Empty)
					{
						consoleOption = _consoleOptionsRepository.Add(consoleOption);
						MessageBus.Current.SendMessage(new ConsoleOptionAddedMessage(consoleOption));
						_consoleOptions.Add(consoleOption);
					}
					else
					{
						_consoleOptionsRepository.Update(consoleOption);
						var currentConsoleOption = _consoleOptions.FirstOrDefault(n => n.Id == consoleOption.Id);
						MessageBus.Current.SendMessage(new ConsoleOptionEditedMessage(consoleOption, currentConsoleOption));
						_consoleOptions.Remove(currentConsoleOption);
						_consoleOptions.Add(consoleOption);
					}

					IsManaging = false;
				}
			});
		}
	}
}
