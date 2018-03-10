using Processes.Core;
using System;

namespace UI.Wpf.Consoles
{
	public interface IConsoleHwndHostFactory
	{
		IConsoleHwndHost Create(IProcess process);
	}

	public class ConsoleHwndHostFactory : IConsoleHwndHostFactory
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleHwndHostFactory()
		{
		}

		public IConsoleHwndHost Create(IProcess process)
		{
			process = process ?? throw new ArgumentNullException(nameof(process), nameof(ConsoleHwndHostFactory));

			return new ConsoleHwndHost(process);
		}
	}
}
