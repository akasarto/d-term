using Consoles.Core;
using WinApi;
using System;
using System.Diagnostics;

namespace Consoles.Processes
{
	public class ConsoleProcess : IConsoleProcess
	{
		private readonly Process _systemProcess;
		private readonly ProcessStartInfo _processStartInfo = null;
		private readonly int _startupTimeoutInSeconds;
		private IntPtr _processMainWindowHandle;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcess(ProcessStartInfo processStartInfo, int startupTimeoutInSeconds)
		{
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(ConsoleProcess));
			if (startupTimeoutInSeconds <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(startupTimeoutInSeconds), startupTimeoutInSeconds, nameof(ConsoleProcess));
			}
			_startupTimeoutInSeconds = startupTimeoutInSeconds;
			_systemProcess = CreateProcess();
		}

		/// <summary>
		/// Process terminated/exited event.
		/// </summary>
		public event ProcessTerminatedHandler Terminated;

		/// <summary>
		/// Gets the process id.
		/// </summary>
		public int Id => _systemProcess.Id;

		/// <summary>
		/// Gets the flag indicating if it has successfully started.
		/// </summary>
		public bool IsStarted { get; private set; }

		/// <summary>
		/// Gets the process main window handle pointer.
		/// </summary>
		public IntPtr MainWindowHandle => _processMainWindowHandle;

		/// <summary>
		/// Gets the option that originated this process.
		/// </summary>
		public ConsoleOption Source { get; internal set; }

		/// <summary>
		/// Starts the process.
		/// </summary>
		public void Start()
		{
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = GetTimeoutInMiliseconds();

			var newProcessStarted = _systemProcess.Start();

			if (newProcessStarted)
			{
				while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
				{
					_processMainWindowHandle = FindHiddenConsoleWindowHandle();

					if (_processMainWindowHandle != IntPtr.Zero)
					{
						IsStarted = true;
						return;
					}
				}

				_systemProcess.Kill();
			}

			IsStarted = false;
		}

		/// <summary>
		/// Create the underlying system process.
		/// </summary>
		/// <returns><see cref="Process"/> instance.</returns>
		private Process CreateProcess()
		{
			var process = new Process()
			{
				EnableRaisingEvents = true,
				StartInfo = _processStartInfo
			};

			process.Exited += (object sender, EventArgs eventArgs) =>
			{
				Terminated?.Invoke(this);
			};

			return process;
		}

		/// <summary>
		/// Attempts to get the underlying process window handle.
		/// </summary>
		/// <returns><see cref="IntPtr"/> for the process main window.</returns>
		private IntPtr FindHiddenConsoleWindowHandle()
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				_systemProcess.Refresh();
				windowHandle = User32Interop.FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = User32Interop.GetWindowThreadProcessId(windowHandle, out processId);
				if (processId == _systemProcess.Id)
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		private int GetTimeoutInMiliseconds() => _startupTimeoutInSeconds * 1000;
	}
}
