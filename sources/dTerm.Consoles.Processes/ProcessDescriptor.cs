using dTerm.Consoles.Core;

namespace dTerm.Consoles.Processes
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		public string Args { get; set; }

		public string FilePath { get; set; }

		public PathType PathType { get; set; }

		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
