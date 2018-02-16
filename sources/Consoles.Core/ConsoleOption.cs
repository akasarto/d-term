using System;

namespace Consoles.Core
{
	public class ConsoleOption
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int OrderIndex { get; set; }
		public string PicturePath { get; set; }
		public BasePath ProcessBasePath { get; set; }
		public string ProcessExecutableName { get; set; }
		public string ProcessStartupArgs { get; set; }
		public DateTime UTCCreation { get; set; }
	}
}
