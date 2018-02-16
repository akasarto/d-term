using Consoles.Core;
using System;

namespace Consoles.Processes
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessDescriptor(ConsoleOption consoleOption)
		{
			ConsoleOption = consoleOption ?? throw new ArgumentNullException(nameof(consoleOption), nameof(ProcessDescriptor));
		}

		/// <summary>
		/// Gets the source console option.
		/// </summary>
		public ConsoleOption ConsoleOption { get; private set; }

		/// <summary>
		/// Gets or sets the timeout that the system will wait until aborting the process start sequence.
		/// </summary>
		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
