using System;

namespace dTerm.Consoles.Core
{
	public interface IConsoleProcess
	{
		int Id { get; }
		bool IsStarted { get; }
		bool IsSupported { get; }
		IntPtr MainWindowHandle { get; }
		void Start();
	}
}
