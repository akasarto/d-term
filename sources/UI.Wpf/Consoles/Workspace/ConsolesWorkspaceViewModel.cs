using Consoles.Core;
using Consoles.Processes;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
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
}
