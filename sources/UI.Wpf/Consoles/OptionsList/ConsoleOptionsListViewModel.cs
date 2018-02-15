using AutoMapper;
using Consoles.Core;
using Consoles.Processes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsListViewModel : BaseViewModel
	{
		//
		private ReactiveList<ConsoleEntity> _consoleEntities;
		private IReactiveDerivedList<ConsoleOptionViewModel> _consoleViewModels;

		//
		private readonly IConsolesRepository _consolesRepository = null;
		private readonly IConsolesProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsListViewModel(IConsolesRepository consolesRepository, IConsolesProcessService consolesProcessService)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(ConsolesWorkspaceViewModel));
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(ConsolesWorkspaceViewModel));

			_consoleEntities = new ReactiveList<ConsoleEntity>()
			{
				ChangeTrackingEnabled = true
			};

			Consoles = _consoleEntities.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: noteEntity => Mapper.Map<ConsoleOptionViewModel>(noteEntity),
				orderer: (noteX, noteY) => noteX.Index.CompareTo(noteY.Index)
			);

			_consoleViewModels.CountChanged.Subscribe(count =>
			{
			});
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
		/// Show the settings window.
		/// </summary>
		public ReactiveCommand ShowSettingsCommand { get; protected set; }

		/// <summary>
		/// Create a new console instance.
		/// </summary>
		public ReactiveCommand<ConsoleOptionViewModel, Unit> CreateInstanceCommand { get; protected set; }

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

		/// <summary>
		/// Wire up commands with their respective actions.
		/// </summary>
		private void SetupCommands()
		{
			ShowSettingsCommand = ReactiveCommand.Create(() =>
			{
				var view = new ConsoleSettingsView()
				{
					Owner = Application.Current.MainWindow,
					DataContext = new ConsoleSettingsViewModel(_consolesRepository),
					WindowStartupLocation = WindowStartupLocation.CenterOwner,
					ShowInTaskbar = false
				};

				view.ShowDialog();
			});

			CreateInstanceCommand = ReactiveCommand.Create<ConsoleOptionViewModel>((consoleViewModel) =>
			{
				var consoleProcess = _consolesProcessService.Create(new ProcessDescriptor()
				{
					PathBuilder = consoleViewModel.ProcessPathBuilder,
					ExeFilename = consoleViewModel.ProcessPathExeFilename,
					ExeStartupArgs = consoleViewModel.ProcessPathExeStartupArgs
				});

				//consoleProcess.Start();

				var consoleInstanceViewModel = new ConsoleIProcessnstanceViewModel(consoleProcess)
				{
					Name = consoleViewModel.Name
				};

				//Instances.Add(consoleInstanceViewModel);
			});
		}
	}
}
