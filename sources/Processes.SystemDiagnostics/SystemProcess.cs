using Processes.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

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

		public string MainWindowClassName { get; private set; }

		public IntPtr MainWindowHandle { get; private set; }

		public IntPtr ParentHandle { get; set; }

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

					var result = GetProcessInfo(_process);

					if (result.mainWindowHandle != IntPtr.Zero)
					{
						ThreadId = result.threadId;
						MainWindowClassName = GetWindowClassName(result.mainWindowHandle);
						MainWindowHandle = result.mainWindowHandle;
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

		private static (uint threadId, IntPtr mainWindowHandle) GetProcessInfo(Process consoleProcess)
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out processId);
				if (processId == consoleProcess.Id)
				{
					return (threadId, windowHandle);
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return (0, IntPtr.Zero);
		}

		private static string GetWindowClassName(IntPtr hWnd)
		{
			int outLength;
			var stringBuilder = new StringBuilder(256);

			outLength = GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);

			if (outLength != 0)
			{
				return stringBuilder.ToString();
			}

			return string.Empty;
		}

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
	}
}
