using dTerm.Consoles.Core;
using dTerm.UI.Wpf.Infrastructure;
using ReactiveUI;
using System;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ShellViewModel : ReactiveObject
	{
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ShellViewModel(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ShellViewModel));

			CreateConsole = ReactiveCommand.Create(CreateConsoleExecute);
		}

		public ReactiveCommand CreateConsole { get; protected set; }

		ConsoleHwndHost _consoleHwndHost;
		public ConsoleHwndHost ConsoleHwndHost
		{
			get { return _consoleHwndHost; }
			set { this.RaiseAndSetIfChanged(ref _consoleHwndHost, value); }
		}

		private void CreateConsoleExecute()
		{
			/*
			var instance = _consoleProcessService.Create(new ProcessDescriptor()
			{
				FilePath = @"/cmd.exe",
				PathType = PathType.SystemPathVar
			});

			instance.Start();

			ConsoleHwndHost = new ConsoleHwndHost(instance);
			*/
		}
	}
}
