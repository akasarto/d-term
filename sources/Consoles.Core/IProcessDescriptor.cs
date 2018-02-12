namespace Consoles.Core
{
	public interface IProcessDescriptor
	{
		string Args { get; }
		string FilePath { get; }
		PathBuilder PathType { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
