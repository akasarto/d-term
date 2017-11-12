using System;

namespace dTerm.Core
{
	public class ProcessEventArgs : EventArgs
	{
		public ProcessEventArgs(ProcessStatus status)
		{
			Status = status;
		}

		public ProcessStatus Status { get; private set; }
	}
}
