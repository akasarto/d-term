using System;

namespace Processes.Core
{
	public interface IProcess : IDisposable
	{
		event EventHandler Exited;

		int Id { get; }
		IntPtr MainModuleHandle { get; }
		string MainWindowClassName { get; }
		IntPtr MainWindowHandle { get; }
		IntPtr ParentHandle { get; set; }
		uint ThreadId { get; }
		bool Start(int startupTimeoutInSeconds = 3);
		void Stop();
	}
}
