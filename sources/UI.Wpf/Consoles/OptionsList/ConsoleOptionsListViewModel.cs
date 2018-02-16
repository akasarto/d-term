using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsListViewModel : BaseViewModel
	{
		private ReactiveList<ConsoleEntity> _consoleEntities;
		private IReactiveDerivedList<ConsoleOptionViewModel> _consoleViewModels;

		//
		private readonly IConsolesRepository _consolesRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsListViewModel(IConsolesRepository consolesRepository)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(ConsolesWorkspaceViewModel));

			//
			_consoleEntities = new ReactiveList<ConsoleEntity>()
			{
				ChangeTrackingEnabled = true
			};

			//
			Consoles = _consoleEntities.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: noteEntity => Mapper.Map<ConsoleOptionViewModel>(noteEntity),
				scheduler: RxApp.MainThreadScheduler
			);
		}

		/// <summary>
		/// Current consoles list
		/// </summary>
		public IReactiveDerivedList<ConsoleOptionViewModel> Consoles
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
			}).Subscribe(items =>
			{
				_consoleEntities.AddRange(items);
			});
		}
	}
}
