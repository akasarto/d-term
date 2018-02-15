using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessInstanceViewModel
	{
		private readonly IConsoleProcess _consoleProcess = null;
		private readonly ConsoleHwndHost _processHost = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessInstanceViewModel(IConsoleProcess consoleProcess)
		{
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleProcessInstanceViewModel));

			_processHost = new ConsoleHwndHost(_consoleProcess);
		}

		/// <summary>
		/// Gets or sets the instance name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the underlying process host interop handler.
		/// </summary>
		public ConsoleHwndHost ProcessHost => _processHost;

		/// <summary>
		/// Gets the process id.
		/// </summary>
		public int ProcessId => _consoleProcess.Id;

		/// <summary>
		/// Event raised each time a console process is terminated.
		/// </summary>
		private void OnConsoleProcessTerminated(int processId)
		{
			//var instance = Instances.Where(i => i.ProcessId == processId).SingleOrDefault();

			//if (instance != null)
			//{
			//	Instances.Remove(instance);
			//}
		}
	}
}
