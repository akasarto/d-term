namespace Consoles.Core
{
	public interface IConsoleProcessService
	{
		bool CanCreate(BasePath processBasePath, string processExecutableName);
		IConsoleProcess Create(IProcessDescriptor processDescriptor);
	}
}
