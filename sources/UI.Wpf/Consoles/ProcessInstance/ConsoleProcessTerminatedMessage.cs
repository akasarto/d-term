using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessTerminatedMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessTerminatedMessage(IConsoleProcess consoleProcess)
		{
			Process = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleProcessTerminatedMessage));
		}

		public IConsoleProcess Process { get; private set; }
	}
}
