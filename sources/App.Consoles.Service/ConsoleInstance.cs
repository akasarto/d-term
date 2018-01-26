using App.Consoles.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace App.Consoles.Service
{
	public class ConsoleInstance : IConsoleInstance
	{
		private readonly Guid _id = Guid.NewGuid();
		private readonly Process _systemProcess;
		private readonly ProcessStartInfo _processStartInfo = null;
		private readonly int _startupTimeoutInSeconds;
		private IntPtr _processMainWindowHandle;

		public ConsoleInstance(ProcessStartInfo processStartInfo, int startupTimeoutInSeconds)
		{
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(ConsoleInstance));
			if (startupTimeoutInSeconds <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(startupTimeoutInSeconds), startupTimeoutInSeconds, nameof(ConsoleInstance));
			}
			_startupTimeoutInSeconds = startupTimeoutInSeconds;
			_systemProcess = CreateProcess();
		}

		public Guid Id => _id;

		public int ProcessId => _systemProcess.Id;

		public IntPtr MainWindowHandle => _processMainWindowHandle;

		public IntPtr ProcessHandle => _systemProcess.Handle;

		public bool Start()
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
						return true;
					}
				}

				_systemProcess.Kill();
			}

			return false;
		}

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		private Process CreateProcess()
		{
			var process = new Process()
			{
				EnableRaisingEvents = true,
				StartInfo = _processStartInfo
			};

			return process;
		}

		private IntPtr FindHiddenConsoleWindowHandle()
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				_systemProcess.Refresh();
				windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out processId);
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
