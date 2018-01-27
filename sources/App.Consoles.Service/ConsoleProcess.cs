using App.Consoles.Core;
using App.Win32Api;
using System;
using System.Diagnostics;
using System.IO;

namespace App.Consoles.Service
{
	public class ConsoleProcess : IConsoleProcess
	{
		private readonly Process _systemProcess;
		private readonly ProcessStartInfo _processStartInfo = null;
		private readonly int _startupTimeoutInSeconds;
		private IntPtr _processMainWindowHandle;

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

		public int Id => _systemProcess.Id;

		public bool IsStarted { get; private set; }

		public bool IsSupported
		{
			get
			{
				var fileName = _processStartInfo.FileName;

				if (string.IsNullOrWhiteSpace(fileName))
				{
					return new FileInfo(fileName).Exists;
				}

				return false;
			}
		}

		public IntPtr MainWindowHandle => _processMainWindowHandle;

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
				windowHandle = NativeMethods.FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = NativeMethods.GetWindowThreadProcessId(windowHandle, out processId);
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
