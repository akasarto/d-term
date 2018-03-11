using Processes.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Processes.SystemDiagnostics
{
	[DesignerCategory("Code")]
	public class SystemProcess : IProcess
	{
		private readonly Process _process;
		private readonly IProcessTracker _processTracker;
		private bool _isStarted;

		public event EventHandler Exited;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SystemProcess(ProcessStartInfo processStartInfo, IProcessTracker processTracker)
		{
			processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(SystemProcess));
			_processTracker = processTracker ?? throw new ArgumentNullException(nameof(processTracker), nameof(SystemProcess));

			_process = new Process()
			{
				StartInfo = processStartInfo,
				EnableRaisingEvents = true
			};

			_process.Exited += OnProcessExited;
		}

		public int Id => _process.Id;

		public IntPtr MainModuleHandle { get; private set; }

		public IntPtr MainWindowHandle { get; private set; }

		public uint ThreadId { get; private set; }

		public bool Start(int startupTimeoutInSeconds = 3)
		{
			if (_isStarted)
			{
				throw new ArgumentException(nameof(SystemProcess), "The process is already started.");
			}

			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = startupTimeoutInSeconds * 1000;

			var newProcessStarted = _process.Start();

			if (newProcessStarted)
			{
				while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
				{
					_process.Refresh();

					var mainWindowHandle = _process.MainWindowHandle;

					if (mainWindowHandle != IntPtr.Zero)
					{
						MainWindowHandle = mainWindowHandle;
						_processTracker.Track(_process.Id);
						_isStarted = true;
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
