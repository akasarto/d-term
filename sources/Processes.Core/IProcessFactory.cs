namespace Processes.Core
{
	public interface IProcessFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcess Create(IProcessDescriptor processDescriptor);
	}
}
