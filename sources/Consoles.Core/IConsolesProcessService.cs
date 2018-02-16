namespace Consoles.Core
{
	public interface IConsolesProcessService
	{
		bool CanCreate(BasePath processBasePath, string processExecutableName);
		IConsoleProcess Create(IProcessDescriptor processDescriptor);
	}
}
