using Consoles.Core;

namespace Consoles.Processes
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		public string Args { get; set; }

		public string FilePath { get; set; }

		public PathBuilder PathType { get; set; }

		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
