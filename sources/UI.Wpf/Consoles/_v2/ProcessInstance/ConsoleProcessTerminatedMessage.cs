using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessTerminatedMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessTerminatedMessage(IConsoleProcess terminatedConsoleProcess)
		{
			TerminatedConsoleProcess = terminatedConsoleProcess ?? throw new ArgumentNullException(nameof(terminatedConsoleProcess), nameof(ConsoleProcessTerminatedMessage));
		}

		/// <summary>
		/// Gets the terminated console process instance.
		/// </summary>
		public IConsoleProcess TerminatedConsoleProcess { get; private set; }
	}
}
