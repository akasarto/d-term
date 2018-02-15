using Consoles.Core;
using System;

namespace Consoles.Process
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		public ProcessDescriptor(ConsoleEntity consoleEntity)
		{
			Console = consoleEntity ?? throw new ArgumentNullException(nameof(consoleEntity), nameof(ProcessDescriptor));
		}

		public ConsoleEntity Console { get; private set; }

		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
