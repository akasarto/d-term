namespace Consoles.Core
{
	public interface IProcessDescriptor
	{
		ConsoleEntity Console { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
