using Processes.Core;
using System;
using System.Diagnostics;

namespace Processes.SystemDiagnostics
{
	/// <summary>
	/// Represents a system <see cref="Process"/> instance in the system.
	/// </summary>
	public class ProcessInstance : IProcessInstance
	{
		//
		private readonly Process _systemProcess;
		private readonly ProcessStartInfo _processStartInfo = null;
		private readonly int _startupTimeoutInSeconds;
		private IntPtr _processMainWindowHandle;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstance(ProcessStartInfo processStartInfo, int startupTimeoutInSeconds)
		{
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(ProcessInstance));
			_startupTimeoutInSeconds = startupTimeoutInSeconds <= 0 ? 3 : startupTimeoutInSeconds;
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
					_processMainWindowHandle = Win32Api.FindHiddenProcessWindowHandle(_systemProcess);

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

		public void Dispose()
		{
			_systemProcess?.Dispose();
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
		/// Get the amount of time to wait for the process to start.
		/// </summary>
		/// <returns>Timeout in miliseconds.</returns>
		private int GetTimeoutInMiliseconds() => _startupTimeoutInSeconds * 1000;
	}
}
