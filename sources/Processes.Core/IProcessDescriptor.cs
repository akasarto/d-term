namespace Processes.Core
{
#warning review the need of this
	public interface IProcessDescriptor
	{
		ProcessEntity ConsoleOption { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
