using Consoles.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsolesWorkspaceViewModel : BaseViewModel
	{
		//
		private readonly IConsolesRepository _consolesRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesWorkspaceViewModel(IConsolesRepository consolesRepository)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(ConsolesWorkspaceViewModel));

			SetupCommands();
		}

		/// <summary>
		/// Add Note View Model
		/// </summary>
		//public ConsoleSettingsViewModel ConsoleSettingsViewModel => _consoleSettingsViewModel;

		/// <summary>
		/// Edit a note.
		/// </summary>
		public ReactiveCommand ShowSettingsCommand { get; protected set; }

		public void Initialize()
		{

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
