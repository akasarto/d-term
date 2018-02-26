namespace Processes.Core
{
	public interface IProcessPathBuilder
	{
		string Build(ProcessBasePath basePath, string executableName);
	}
}
