using Consoles.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessInstanceViewModel
	{
		private ConsoleHwndHost _processHost;

		//
		private readonly IConsoleProcess _consoleProcess = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessInstanceViewModel(IConsoleProcess consoleProcess)
		{
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleProcessInstanceViewModel));

			_consoleProcess.Terminated += OnProcessTerminated;

			_processHost = new ConsoleHwndHost(_consoleProcess);
		}

		/// <summary>
		/// Gets the underlying process host interop handler.
		/// </summary>
		public ConsoleHwndHost ProcessHost => _processHost;

		/// <summary>
		/// Raised when the underlying process exits.
		/// </summary>
		private void OnProcessTerminated(IConsoleProcess process)
		{
			MessageBus.Current.SendMessage(new ConsoleProcessTerminatedMessage(process));
		}
	}
}
