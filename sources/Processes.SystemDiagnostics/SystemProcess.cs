using Processes.Core;
using System;
using System.Diagnostics;

namespace Processes.SystemDiagnostics
{
	public class SystemProcess : IProcess
	{
		private readonly Process _process;

		public event EventHandler Exited;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SystemProcess(ProcessStartInfo processStartInfo)
		{
			processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(SystemProcess));

			_process = new Process()
			{
				StartInfo = processStartInfo,
				EnableRaisingEvents = true
			};

			_process.Exited += OnProcessExited;
		}

		public int Id => _process.Id;

		public bool Start(int startupTimeoutInSeconds = 3)
		{
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = startupTimeoutInSeconds * 1000;

			var newProcessStarted = _process.Start();

			if (newProcessStarted)
			{
				while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
				{
					_process.Refresh();

					if (_process.MainWindowHandle != IntPtr.Zero)
					{
						return true;
					}
				}

				Stop();
			}

			return false;
		}

		public void Stop() => _process.Kill();

		public void Dispose()
		{
			_process.Exited -= OnProcessExited;

			_process.Dispose();
		}

		private void OnProcessExited(object sender, EventArgs args)
		{
			Exited?.Invoke(this, args);
		}
	}
}
