using System;
using System.Collections.Generic;

namespace Consoles.Core
{
	public interface IConsoleProcess
	{
		event ProcessTerminatedHandler Terminated;

		int Id { get; }
		bool IsStarted { get; }
		IntPtr MainWindowHandle { get; }
		ConsoleOption Source { get; }
		List<int> ThreadIds { get; }
		void Start();
	}
}
