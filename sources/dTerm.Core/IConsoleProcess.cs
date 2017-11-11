using System;
using System.Diagnostics;

namespace dTerm.Core
{
	public interface IConsoleProcess
	{
		event EventHandler<ProcessEventArgs> ProcessStatusChanged;

		string Name { get; set; }

		int PorcessId { get; }

		IntPtr ProcessMainHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		ConsoleType Type { get; set; }

		void Initialize(Action<Process> onMainWindowHandleAccquiredAction = null);

		void Terminate();
	}
}
