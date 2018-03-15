namespace Processes.Core
{
	public interface IProcessesTracker
	{
		void KillAll();
		void Track(int processId);
	}
}
