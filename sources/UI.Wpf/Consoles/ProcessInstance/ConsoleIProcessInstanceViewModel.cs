using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleIProcessInstanceViewModel
	{
		private readonly IConsoleProcess _consoleProcess = null;
		private readonly ConsoleHwndHost _processHost = null;

		public ConsoleIProcessInstanceViewModel(IConsoleProcess consoleProcess)
		{
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleIProcessInstanceViewModel));
			_processHost = new ConsoleHwndHost(_consoleProcess);
		}

		public string Name { get; set; }

		public ConsoleHwndHost ProcessHost => _processHost;
	}
}
