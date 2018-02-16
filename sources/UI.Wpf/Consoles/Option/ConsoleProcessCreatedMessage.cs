using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessCreatedMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessCreatedMessage(IConsoleProcess newConsoleProcess)
		{
			NewConsoleProcess = newConsoleProcess ?? throw new ArgumentNullException(nameof(newConsoleProcess), nameof(ConsoleProcessCreatedMessage));
		}

		/// <summary>
		/// Gets the newly created process.
		/// </summary>
		public IConsoleProcess NewConsoleProcess { get; private set; }
	}
}
