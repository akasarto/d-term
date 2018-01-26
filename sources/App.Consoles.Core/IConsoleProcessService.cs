namespace App.Consoles.Core
{
	public interface IConsoleProcessService
	{
		IConsoleInstance Create(IProcessDescriptor descriptor);
	}
}
