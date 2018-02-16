namespace Consoles.Core
{
	public interface IProcessPathBuilder
	{
		string Build(BasePath basePath, string executableName);
	}
}
