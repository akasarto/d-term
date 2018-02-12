namespace Consoles.Core
{
	public interface IConsolesProcessService
	{
		IConsoleProcess Create(IProcessDescriptor descriptor);
	}
}
