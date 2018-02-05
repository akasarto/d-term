using System;

namespace Consoles.Core
{
	public class Console
	{
		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }
		public string ProcessArgs { get; set; }
		public string ProcessFilePath { get; set; }
		public PathType ProcessPathType { get; set; }
	}
}
