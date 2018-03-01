using System;

namespace Processes.Core
{
	public interface IProcess : IDisposable
	{
		event EventHandler Exited;

		int Id { get; }
		IntPtr MainWindowHandle { get; }
		IntPtr ParentHandle { get; set; }
		bool Start(int startupTimeoutInSeconds = 3);
	}
}
