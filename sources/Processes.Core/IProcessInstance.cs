using System;

namespace Processes.Core
{
	public interface IProcessInstance
	{
		event ProcessTerminatedHandler Terminated;

		int Id { get; }
		bool IsStarted { get; }
		IntPtr MainWindowHandle { get; }
		void Start();
	}
}
