using dTerm.Core;
using dTerm.Core.ProcessStarters;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WinApi.User32;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleInstance : ObservableObject, IConsoleInstance
	{
		private IntPtr _ownerHandle;
		private string _consoleName;
		private ConsoleType _consoleType;
		private ConsoleProcessStartInfo _consoleProcessStartInfo;
		private IntPtr _processMainWindowHandle;
		private Process _systemProcess;
		private int _timeoutSeconds;

		public ConsoleInstance(ConsoleType consoleType, ConsoleProcessStartInfo consoleProcessStartInfo, int timeoutSeconds = 3)
		{
			_consoleType = consoleType;
			_consoleProcessStartInfo = consoleProcessStartInfo ?? throw new ArgumentNullException(nameof(consoleProcessStartInfo), nameof(ConsoleInstance));
			_timeoutSeconds = timeoutSeconds;

			CreateProcess();
		}

		public event EventHandler ProcessTerminated;

		public string Name
		{
			get => _consoleName;
			set => Set(ref _consoleName, value);
		}

		public int ProcessId => _systemProcess.Id;

		public IntPtr ProcessHandle => _systemProcess.Handle;

		public IntPtr ProcessMainWindowHandle => _processMainWindowHandle;

		public ConsoleType Type => _consoleType;

		public bool Initialize()
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

				Terminate();
			}

			return false;
		}

		public void SetOwner(IntPtr ownerHandle)
		{
			_ownerHandle = ownerHandle;
		}

		public void ShowProcessView()
		{
			if (!User32Methods.IsWindowVisible(ProcessMainWindowHandle))
			{
				User32Methods.ShowWindow(_ownerHandle, ShowWindowCommands.SW_SHOW);
			}

			User32Methods.SendMessage(_ownerHandle, ((uint)WM.APP + 0x1), _processMainWindowHandle, IntPtr.Zero);
		}

		public void Terminate()
		{
			if (_systemProcess.HasExited)
			{
				return;
			}

			_systemProcess.Kill();
		}

		private void CreateProcess()
		{
			_systemProcess = new Process()
			{
				EnableRaisingEvents = true,
				StartInfo = _consoleProcessStartInfo
			};

			_systemProcess.Exited += OnSystemProcessExited;
		}

		private void OnSystemProcessExited(object sender, EventArgs e)
		{
			_systemProcess.Exited -= OnSystemProcessExited;
			ProcessTerminated?.Invoke(this, EventArgs.Empty);
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

		[DllImport("user32.dll", ExactSpelling = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
	}
}
