namespace Consoles.Core
{
	public interface IConsoleProcessService
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IConsoleProcess Create(IProcessDescriptor processDescriptor);
	}
}
