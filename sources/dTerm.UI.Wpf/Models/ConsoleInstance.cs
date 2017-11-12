using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WinApi.User32;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleInstance : ObservableObject, IConsoleInstance
	{
		private string _consoleName;
		private ConsoleType _consoleType;
		private ProcessStartInfo _processStartInfo;
		private Process _systemProcess;
		private int _timeoutSeconds;

		public ConsoleInstance(ConsoleType consoleType, ProcessStartInfo processStartInfo, int timeoutSeconds = 5)
		{
			_consoleType = consoleType;
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(ConsoleInstance));
			_timeoutSeconds = timeoutSeconds;

			Configure();
		}

		public event EventHandler<ProcessEventArgs> ProcessStatusChanged;

		public string Name
		{
			get => _consoleName;
			set => Set(ref _consoleName, value);
		}

		public int ProcessId => _systemProcess.Id;

		public IntPtr ProcessHandle => _systemProcess.Handle;

		public IntPtr ProcessMainWindowHandle
		{
			get
			{
				if (_systemProcess.MainWindowHandle == IntPtr.Zero)
				{
					return FindHiddenConsoleWindowHandle();
				}

				return _systemProcess.MainWindowHandle;
			}
		}

		public ConsoleType Type => _consoleType;

		public void Initialize()
		{
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = GetTimeoutInMiliseconds();

			_systemProcess.Start();

			while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
			{
				if (ProcessMainWindowHandle != IntPtr.Zero)
				{
					ProcessStatusChanged?.Invoke(this, new ProcessEventArgs(ProcessStatus.Initialized));
					return;
				}
			}

			ProcessStatusChanged?.Invoke(this, new ProcessEventArgs(ProcessStatus.Timeout));

			Terminate();
		}

		public void Terminate()
		{
			if (_systemProcess.HasExited)
			{
				return;
			}

			_systemProcess.Kill();
			_systemProcess.WaitForExit(GetTimeoutInMiliseconds());
		}

		private void Configure()
		{
			if (string.IsNullOrWhiteSpace(_processStartInfo.WorkingDirectory))
			{
				_processStartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			}

			_processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

			_systemProcess = new Process()
			{
				EnableRaisingEvents = true,
				StartInfo = _processStartInfo
			};

			_systemProcess.Exited += OnSystemProcessExited;
		}

		private void OnSystemProcessExited(object sender, EventArgs e)
		{
			_systemProcess.Exited -= OnSystemProcessExited;
			ProcessStatusChanged?.Invoke(this, new ProcessEventArgs(ProcessStatus.Terminated));
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
				windowHandle = User32Methods.FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out processId);
				if (processId == _systemProcess.Id)
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		private int GetTimeoutInMiliseconds() => _timeoutSeconds * 1000;

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
	}
}
