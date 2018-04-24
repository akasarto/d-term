using System;

namespace Processes.Core
{
	public interface IProcess : IDisposable
	{
		event EventHandler Exited;

		int Id { get; }
		bool HasExited { get; }
		bool Start(int startupTimeoutInSeconds = 3);
		void Kill();
	}
}
