using Consoles.Core;
using Consoles.Processes;
using UI.Wpf.Consoles;
using ReactiveUI;
using System;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ShellViewModel(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ShellViewModel));

			CreateConsole = ReactiveCommand.Create(CreateConsoleExecute);
		}

		public ReactiveList<ConsoleInstanceViewModel> ConsoleInstances { get; set; } = new ReactiveList<ConsoleInstanceViewModel>();

		public ReactiveCommand CreateConsole { get; protected set; }

		private void CreateConsoleExecute()
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

			ConsoleInstances.Add(consoleInstanceViewModel);
		}
	}
}
