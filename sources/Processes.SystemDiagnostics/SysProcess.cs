using Processes.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Processes.SystemDiagnostics
{
	[DesignerCategory("Code")]
	public class SysProcess : Process, IProcess
	{
		private readonly IProcessTracker _processTracker;
		private IntPtr _processMainWindowHandle;
		private IntPtr _parentHandle;
		private bool _isStarted;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SysProcess(IProcessTracker processTracker, ProcessStartInfo processStartInfo)
		{
			_processTracker = processTracker ?? throw new ArgumentNullException(nameof(processTracker), nameof(SysProcess));

			StartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(SysProcess));

			EnableRaisingEvents = true;
		}

		public IntPtr ParentHandle
		{
			get => _parentHandle;
			set => _parentHandle = value;
		}

		public new IntPtr MainWindowHandle => _processMainWindowHandle;

		public bool Start(int startupTimeoutInSeconds = 3)
		{
			if (_isStarted)
			{
				throw new ArgumentException(nameof(SysProcess), "The process is already started.");
			}

			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = startupTimeoutInSeconds * 1000;

			var newProcessStarted = base.Start();

			if (newProcessStarted)
			{
				while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
				{
					_processMainWindowHandle = FindHiddenProcessWindowHandle(this);

					if (_processMainWindowHandle != IntPtr.Zero)
					{
						_processTracker.Track(Id);
						_isStarted = true;

						return true;
					}
				}

				Kill();
			}

			return false;
		}

		private static IntPtr FindHiddenProcessWindowHandle(Process consoleProcess)
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				consoleProcess.Refresh();
				windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out processId);
				if (processId == consoleProcess.Id)
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
	}
}
