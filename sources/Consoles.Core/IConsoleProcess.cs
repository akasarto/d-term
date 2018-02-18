using System;

namespace Consoles.Core
{
	public interface IConsoleProcess
	{
		event ProcessTerminatedHandler Terminated;

		int Id { get; }
		bool IsStarted { get; }
		IntPtr MainWindowHandle { get; }
		ConsoleOption Source { get; }
		void Start();
	}
}
