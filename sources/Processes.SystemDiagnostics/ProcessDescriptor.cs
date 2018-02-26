using Processes.Core;
using System;

namespace Processes.SystemDiagnostics
{
	public class ProcessDescriptor : IProcessDescriptor
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessDescriptor(ProcessEntity consoleOption)
		{
			ConsoleOption = consoleOption ?? throw new ArgumentNullException(nameof(consoleOption), nameof(ProcessDescriptor));
		}

		/// <summary>
		/// Gets the source console option.
		/// </summary>
		public ProcessEntity ConsoleOption { get; private set; }

		/// <summary>
		/// Gets or sets the timeout that the system will wait until aborting the process start sequence.
		/// </summary>
		public int StartupTimeoutInSeconds { get; set; } = 3;
	}
}
