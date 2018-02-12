using AutoMapper;
using Consoles.Core;
using Consoles.Processes;
using Dragablz.Dockablz;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		//
		private IInputElement _consolesControl = null;
		private ReactiveList<ConsoleEntity> _consoleEntities;
		private IReactiveDerivedList<ConsoleViewModel> _consoleViewModels;
		private ReactiveList<ConsoleInstanceViewModel> _consoleInstanceViewModels;

		//
		private readonly IConsolesRepository _consolesRepository = null;
		private readonly IConsolesProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(IConsolesRepository consolesRepository, IConsolesProcessService consolesProcessService)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(ConsolesWorkspaceViewModel));
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(ConsolesWorkspaceViewModel));

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

			_consoleInstanceViewModels = new ReactiveList<ConsoleInstanceViewModel>()
			{
				//ChangeTrackingEnabled = true
			};

			_consoleInstanceViewModels.Changed.Subscribe(instances =>
			{
				Layout.TileFloatingItemsCommand.Execute(null, _consolesControl);
			});

			SetupCommands();
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
		/// Current consoles instances list
		/// </summary>
		public ReactiveList<ConsoleInstanceViewModel> Instances
		{
			get => _consoleInstanceViewModels;
			set => this.RaiseAndSetIfChanged(ref _consoleInstanceViewModels, value);
		}

		/// <summary>
		/// Show the settings window.
		/// </summary>
		public ReactiveCommand ShowSettingsCommand { get; protected set; }

		/// <summary>
		/// Create a new console instance.
		/// </summary>
		public ReactiveCommand<ConsoleViewModel, Unit> CreateInstanceCommand { get; protected set; }

		/// <summary>
		/// Initialize the model.
		/// </summary>
		public void Initialize(IInputElement consolesControl)
		{
			_consolesControl = consolesControl;

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
					WindowStartupLocation = WindowStartupLocation.CenterScreen,
					DataContext = new ConsoleSettingsViewModel(_consolesRepository)
				};

				view.ShowDialog();
			});

			CreateInstanceCommand = ReactiveCommand.Create<ConsoleViewModel>((consoleViewModel) =>
			{
				var consoleProcess = _consolesProcessService.Create(new ProcessDescriptor()
				{
					PathBuilder = consoleViewModel.ProcessPathBuilder,
					ExeFilename = consoleViewModel.ProcessPathExeFilename,
					ExeStartupArgs = consoleViewModel.ProcessPathExeStartupArgs
				});

				consoleProcess.Start();

				var consoleInstanceViewModel = new ConsoleInstanceViewModel(consoleProcess)
				{
					Name = consoleViewModel.Name
				};

				Instances.Add(consoleInstanceViewModel);
			});
		}
	}
}
