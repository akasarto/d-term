using System;
using System.Diagnostics;

namespace dTerm.Core
{
	public interface IConsoleProcess
	{
		event EventHandler<ProcessEventArgs> ProcessStatusChanged;

		ConsoleType ConsoleType { get; }

		int PorcessId { get; }

		IntPtr ProcessHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		void Initialize(Action<Process> onMainWindowHandleAccquiredAction = null);

		void Terminate();
	}
}
