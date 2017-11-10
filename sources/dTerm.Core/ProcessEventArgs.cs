namespace dTerm.Core
{
	public class ProcessEventArgs
	{
		public ProcessEventArgs(ProcessStatus status)
		{
			Status = status;
		}

		public ProcessStatus Status { get; private set; }
	}
}
