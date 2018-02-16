using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsoleSettingsDialogViewModel : BaseViewModel
	{
		//
		private ReactiveList<ConsoleOption> _consoleOptions;
		private IReactiveDerivedList<ConsoleOptionViewModel> _consoleOptionViewModels;

		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleSettingsDialogViewModel(IConsoleOptionsRepository consoleOptionsRepository)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? throw new ArgumentNullException(nameof(consoleOptionsRepository), nameof(ConsoleSettingsDialogViewModel));

			_consoleOptions = new ReactiveList<ConsoleOption>()
			{
				ChangeTrackingEnabled = true
			};

			ConsoleOptions = _consoleOptions.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: noteEntity => Mapper.Map<ConsoleOptionViewModel>(noteEntity),
				orderer: (noteX, noteY) => noteX.OrderIndex.CompareTo(noteY.OrderIndex),
				scheduler: RxApp.MainThreadScheduler
			);

			_consoleOptionViewModels.CountChanged.Subscribe(count =>
			{
			});
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
	}
}
