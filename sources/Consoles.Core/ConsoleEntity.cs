using System;

namespace Consoles.Core
{
	public class ConsoleEntity
	{
		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }
		public string IconPath { get; set; }
		public PathBuilder ProcessPathBuilder { get; set; }
		public string ProcessPathExeFilename { get; set; }
		public string ProcessPathExeArgs { get; set; }
		public DateTime UTCCreation { get; set; }
	}
}
