namespace Consoles.Core
{
	public interface IProcessTracker
	{
		void Add(int processId);
		void KillAll();
	}
}
