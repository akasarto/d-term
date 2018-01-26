using System;

namespace App.Consoles.Core
{
	public interface IConsoleInstance
	{
		Guid Id { get; }
		int ProcessId { get; }
		IntPtr MainWindowHandle { get; }
		IntPtr ProcessHandle { get; }
		bool Start();
	}
}
