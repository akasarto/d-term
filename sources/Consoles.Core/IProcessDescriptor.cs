namespace Consoles.Core
{
	public interface IProcessDescriptor
	{
		ConsoleEntity ConsoleOption { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
