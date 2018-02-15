using Consoles.Core;

namespace Consoles.Processes
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		public string Name { get; set; }

		public string ExeFilename { get; set; }

		public string ExeStartupArgs { get; set; }

		public PathBuilder PathBuilder { get; set; }

		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
