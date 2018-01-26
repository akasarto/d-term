using App.Consoles.Core;

namespace App.Consoles.Service
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		public string Args { get; set; }

		public string FilePath { get; set; }

		public PathType PathType { get; set; }

		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
