namespace Consoles.Core
{
	public interface IProcessDescriptor
	{
		string ExeStartupArgs { get; }
		string ExeFilename { get; }
		PathBuilder PathBuilder { get; }
		int StartupTimeoutInSeconds { get; }
	}
}
