using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleIProcessnstanceViewModel
	{
		private readonly IConsoleProcess _consoleProcess = null;
		private readonly ConsoleHwndHost _processHost = null;

		public ConsoleIProcessnstanceViewModel(IConsoleProcess consoleProcess)
		{
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleIProcessnstanceViewModel));
			_processHost = new ConsoleHwndHost(_consoleProcess);
		}

		public string Name { get; set; }

		public ConsoleHwndHost ProcessHost => _processHost;
	}
}
