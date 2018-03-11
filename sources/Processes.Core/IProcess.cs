using System;

namespace Processes.Core
{
	public interface IProcess : IDisposable
	{
		event EventHandler Exited;

		int Id { get; }
		IntPtr MainWindowHandle { get; }
		bool Start(int startupTimeoutInSeconds = 3);
		void Stop();
	}
}
