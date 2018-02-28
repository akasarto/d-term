using System;

namespace Processes.Core
{
	public interface IProcessInstance : IDisposable
	{
		event ProcessTerminatedHandler Terminated;

		int Id { get; }
		bool IsStarted { get; }
		IntPtr MainWindowHandle { get; }
		IntPtr ParentHandle { get; set; }
		void Start();
	}
}
