using Consoles.Core;
using Consoles.Processes;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsolesAreaViewModel : ReactiveObject
	{
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ConsolesAreaViewModel(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ConsolesAreaViewModel));

			CreateInstance = ReactiveCommand.Create(CreateInstanceAction);
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
