using System;

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

		void Initialize();

		void Terminate();
	}
}
