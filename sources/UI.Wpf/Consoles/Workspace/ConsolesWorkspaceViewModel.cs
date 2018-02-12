using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		//
		private ReactiveList<ConsoleEntity> _consoleEntities;
		private IReactiveDerivedList<ConsoleViewModel> _consoleViewModels;

		//
		private readonly IConsolesRepository _consolesRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(IConsolesRepository consolesRepository)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(ConsolesWorkspaceViewModel));

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
		/// Edit a note.
		/// </summary>
		public ReactiveCommand ShowSettingsCommand { get; protected set; }

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
					WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
					DataContext = new ConsoleSettingsViewModel(_consolesRepository)
				};

				view.ShowDialog();
			});
		}
	}



	/*
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ConsolesWorkspaceViewModel(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ConsolesWorkspaceViewModel));

			CreateInstance = ReactiveCommand.Create(CreateInstanceAction);

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(x => x).Subscribe(viewModel =>
				{

				}));
			});
		}

		public ReactiveCommand CreateInstance { get; protected set; }

		public ReactiveList<ConsoleInstanceViewModel> Instances { get; set; } = new ReactiveList<ConsoleInstanceViewModel>();

		public void Initialize()
		{
		}

		private void CreateInstanceAction()
		{
			var consoleProcess = _consoleProcessService.Create(new ProcessDescriptor()
			{
				FilePath = @"/cmd.exe",
				PathType = PathType.SystemPathVar
			});

			//consoleProcess.Start();

			var consoleInstanceViewModel = new ConsoleInstanceViewModel(consoleProcess)
			{
				Name = DateTime.Now.Millisecond.ToString()
			};

			Instances.Add(consoleInstanceViewModel);
		}
	}
	*/
}
