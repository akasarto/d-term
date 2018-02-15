using Consoles.Processes;
using System;

namespace UI.Wpf.Consoles
{
	public class CreateConsoleInstanceMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public CreateConsoleInstanceMessage(ProcessDescriptor processDescriptor)
		{
			ProcessDescriptor = processDescriptor ?? throw new ArgumentNullException(nameof(processDescriptor), nameof(CreateConsoleInstanceMessage));
		}

		public ProcessDescriptor ProcessDescriptor { get; private set; }
	}
}
