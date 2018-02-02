using dTerm.Consoles.Core;
using dTerm.Consoles.Processes;
using dTerm.UI.Wpf.Consoles;
using ReactiveUI;
using System;

namespace dTerm.UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ShellViewModel(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ShellViewModel));

			CreateConsole = ReactiveCommand.Create(CreateConsoleExecute);
		}


		private ConsoleHwndHost _tmp = null;
		public ConsoleHwndHost Tmp
		{
			get => _tmp;
			set => this.RaiseAndSetIfChanged(ref _tmp, value);
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

			consoleProcess.Start();

			Tmp = new ConsoleHwndHost(consoleProcess);
			//var consoleInstanceViewModel = new ConsoleInstanceViewModel(consoleProcess)
			//{
			//	Name = DateTime.Now.Millisecond.ToString()
			//};

			//ConsoleInstances.Add(consoleInstanceViewModel);
		}
	}
}
