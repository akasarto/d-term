using System;

namespace dTerm.Core.Processes
{
	public interface ITermProcess
	{
		event EventHandler Started;
		event EventHandler Killed;

		int Id { get; }

		bool IsRunning { get; }

		IntPtr ProcessHandle { get; }

		IntPtr MainWindowHandle { get; }

		bool Start();

		bool Kill();
	}
}
