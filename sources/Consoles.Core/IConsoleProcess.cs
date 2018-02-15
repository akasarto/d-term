using System;

namespace Consoles.Core
{
	public interface IConsoleProcess
	{
		int Id { get; }
		bool IsStarted { get; }
		bool IsSupported { get; }
		IntPtr MainWindowHandle { get; }
		ConsoleEntity SourceSpecifications { get; }
		void Start();
	}
}
