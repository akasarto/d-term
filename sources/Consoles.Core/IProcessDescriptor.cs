namespace Consoles.Core
{
	public interface IProcessDescriptor
	{
		ConsoleOption ConsoleOption { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
