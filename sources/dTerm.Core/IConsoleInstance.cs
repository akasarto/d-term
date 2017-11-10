using System;

namespace dTerm.Core
{
	public interface IConsoleInstance
	{
		event EventHandler<ProcessEventArgs> ProcessStatusChanged;

		string Name { get; set; }

		int PorcessId { get; }

		IntPtr ProcessMainHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		ConsoleType Type { get; set; }

		void Initialize();

		void Terminate();
	}
}
