using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsListViewModel : BaseViewModel
	{
		private ReactiveList<ConsoleOption> _consoleOptions;
		private IReactiveDerivedList<ConsoleOptionViewModel> _consoleOptionViewModels;

		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsListViewModel(IConsoleOptionsRepository consoleOptionsRepository)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? throw new ArgumentNullException(nameof(consoleOptionsRepository), nameof(ConsolesWorkspaceViewModel));

			_consoleOptions = new ReactiveList<ConsoleOption>()
			{
				ChangeTrackingEnabled = true
			};

			ConsoleOptions = _consoleOptions.CreateDerivedCollection(
				filter: option => true,
				selector: option => Mapper.Map<ConsoleOptionViewModel>(option),
				scheduler: RxApp.MainThreadScheduler
			);

			SetupMessageBus();
		}

		/// <summary>
		/// Get the current console options list.
		/// </summary>
		public IReactiveDerivedList<ConsoleOptionViewModel> ConsoleOptions
		{
			get => _consoleOptionViewModels;
			set => this.RaiseAndSetIfChanged(ref _consoleOptionViewModels, value);
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


		/// <summary>
		/// Wireup the messages this view model will listen to.
		/// </summary>
		private void SetupMessageBus()
		{
			MessageBus.Current.Listen<ConsoleOptionAddedMessage>().Subscribe(message =>
			{
				_consoleOptions.Add(message.NewConsoleOption);
			});

			MessageBus.Current.Listen<ConsoleOptionDeletedMessage>().Subscribe(message =>
			{
				var noteEntity = _consoleOptions.FirstOrDefault(n => n.Id == message.DeletedConsoleOption.Id);

				if (noteEntity != null)
				{
					_consoleOptions.Remove(noteEntity);
				}
			});

			MessageBus.Current.Listen<ConsoleOptionEditedMessage>().Subscribe(message =>
			{
				var consoleOption = _consoleOptions.FirstOrDefault(n => n.Id == message.OldConsoleOption.Id);

				if (consoleOption != null)
				{
					_consoleOptions.Remove(consoleOption);
					_consoleOptions.Add(message.NewConsoleOption);
				}
			});
		}
	}
}
