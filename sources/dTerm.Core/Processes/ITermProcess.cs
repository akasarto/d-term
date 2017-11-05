using System;

namespace dTerm.Core.Processes
{
	public interface ITermProcess
	{
		event EventHandler Started;
		event EventHandler Killed;

		int Id { get; }

		IntPtr Handle { get; }

		IntPtr MainWindowHandle { get; }

		bool Start();

		bool Kill();
	}
}
