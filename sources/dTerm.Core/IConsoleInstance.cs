using System;
using System.Diagnostics;

namespace dTerm.Core
{
	public interface IConsoleInstance
	{
		event EventHandler<ProcessEventArgs> ProcessStatusChanged;

		string Name { get; set; }

		int ProcessId { get; }

		IntPtr ProcessHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		ConsoleType Type { get; }

		void Initialize(Action<Process> onMainWindowHandleAccquiredAction = null);

		void Terminate();
	}
}
