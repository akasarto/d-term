using System;

namespace Processes.Core
{
	public interface IProcessesTracker : IDisposable
	{
		void KillAll();
		void Track(int processId);
	}
}
