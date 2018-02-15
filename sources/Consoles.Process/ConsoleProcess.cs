using Consoles.Core;
using Shared.Kernel;
using System;
using System.IO;
using Sys = System.Diagnostics;

namespace Consoles.Process
{
	public class ConsoleProcess : IConsoleProcess
	{
		private readonly Sys.Process _systemProcess;
		private readonly Sys.ProcessStartInfo _processStartInfo = null;
		private readonly int _startupTimeoutInSeconds;
		private IntPtr _processMainWindowHandle;

		public ConsoleProcess(Sys.ProcessStartInfo processStartInfo, int startupTimeoutInSeconds)
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

		public ConsoleEntity SourceSpecifications { get; internal set; }

		public void Start()
		{
			var processStopwatch = Sys.Stopwatch.StartNew();
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

		private Sys.Process CreateProcess()
		{
			var process = new Sys.Process()
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
				windowHandle = Win32Api.FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = Win32Api.GetWindowThreadProcessId(windowHandle, out processId);
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
