using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsoleSettingsViewModel : BaseViewModel
	{
		//
		private ReactiveList<ConsoleEntity> _consoleEntities;
		private IReactiveDerivedList<ConsoleViewModel> _consoleViewModels;

		//
		private readonly IConsolesRepository _consolesRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleSettingsViewModel(IConsolesRepository consolesRepository)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(ConsoleSettingsViewModel));

			_consoleEntities = new ReactiveList<ConsoleEntity>()
			{
				ChangeTrackingEnabled = true
			};

			Consoles = _consoleEntities.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: noteEntity => Mapper.Map<ConsoleViewModel>(noteEntity),
				orderer: (noteX, noteY) => noteX.Index.CompareTo(noteY.Index)
			);

			_consoleViewModels.CountChanged.Subscribe(count =>
			{
			});
		}

		/// <summary>
		/// Current consoles list
		/// </summary>
		public IReactiveDerivedList<ConsoleViewModel> Consoles
		{
			get => _consoleViewModels;
			set => this.RaiseAndSetIfChanged(ref _consoleViewModels, value);
		}

		/// <summary>
		/// Initialize the model.
		/// </summary>
		public void Initialize()
		{
			Observable.Start(() =>
			{
				var entities = _consolesRepository.GetAll();
				return entities;
			}, RxApp.MainThreadScheduler)
			.Subscribe(items =>
			{
				_consoleEntities.AddRange(items);
			});
		}
	}
}
