namespace App.Consoles.Core
{
	public interface IProcessDescriptor
	{
		string Args { get; }
		string FilePath { get; }
		PathType PathType { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
