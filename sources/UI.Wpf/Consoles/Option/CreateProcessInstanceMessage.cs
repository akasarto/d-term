using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class CreateProcessInstanceMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public CreateProcessInstanceMessage(IConsoleProcess consoleProcess)
		{
			Process = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(CreateProcessInstanceMessage));
		}

		public IConsoleProcess Process { get; private set; }
	}
}
