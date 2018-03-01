using System;

namespace Processes.Core
{
	/// <summary>
	/// Process interface.
	/// </summary>
	public interface IProcess : IDisposable
	{
		event ProcessTerminatedHandler Terminated;

		int Id { get; }
		IntPtr MainWindowHandle { get; }
		IntPtr ParentHandle { get; set; }
		bool Start();
	}
}
