using dTerm.Core.Entities;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Diagnostics;
using WinApi.User32;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleInstance : ObservableObject, IConsoleInstance
	{
		private string _name;
		private ConsoleType _consoleType;
		private ProcessStartInfo _processStartInfo;
		private Process _systemProcess;
		private int _timeoutSeconds;

		public ConsoleInstance(ProcessStartInfo processStartInfo, int timeoutSeconds = 5)
		{
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(ConsoleInstance));
			_timeoutSeconds = timeoutSeconds;

			Configure();
		}

		public event EventHandler Started;
		public event EventHandler Killed;

		public string Name
		{
			get => _name;
			set => Set(ref _name, value);
		}

		public int PorcessId => _systemProcess?.Id ?? 0;

		public bool IsRunning => !(_systemProcess?.HasExited ?? true);

		public IntPtr ProcessMainHandle => _systemProcess?.Handle ?? IntPtr.Zero;

		public IntPtr ProcessMainWindowHandle => _systemProcess?.MainWindowHandle ?? IntPtr.Zero;

		public ConsoleType Type
		{
			get => _consoleType;
			set => Set(ref _consoleType, value);
		}

		public void Kill()
		{
			if (!_systemProcess.HasExited)
			{
				_systemProcess.Kill();
				_systemProcess.WaitForExit(GetTimeoutInMiliseconds());
			}

			_systemProcess = null;
		}

		public void Start()
		{
			var isStarted = false;
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = GetTimeoutInMiliseconds();

			_systemProcess.Start();

			while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
			{
				isStarted = _systemProcess?.MainWindowHandle != IntPtr.Zero && !_systemProcess.HasExited;

				if (isStarted)
				{
					Started?.Invoke(this, EventArgs.Empty);
				}
			}

			Kill();
		}

		public void TransferOwnership(IntPtr ownerWindowHandle)
		{
			IntPtr targetWindoHandle = _systemProcess.MainWindowHandle;

			SetOwner(targetWindoHandle, ownerWindowHandle);
			RemoveFromTaskbar(targetWindoHandle);
			MakeToolWindow(targetWindoHandle);
		}

		private void SetOwner(IntPtr targetWindoHandle, IntPtr newOwnerHandle)
		{
			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWLP_HWNDPARENT, newOwnerHandle);
		}

		private void RemoveFromTaskbar(IntPtr targetWindoHandle)
		{
			var newStyle = (WindowExStyles)User32Helpers.GetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_EXSTYLE);
			newStyle &= ~WindowExStyles.WS_EX_APPWINDOW;
			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_EXSTYLE, new IntPtr((long)newStyle));
		}

		private void MakeToolWindow(IntPtr targetWindoHandle)
		{
			var newStyle = (WindowStyles)User32Helpers.GetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_STYLE);
			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			newStyle &= ~WindowStyles.WS_MINIMIZEBOX;
			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		}

		private void Configure()
		{
			if (string.IsNullOrWhiteSpace(_processStartInfo.WorkingDirectory))
			{
				_processStartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			}

			_systemProcess = new Process()
			{
				EnableRaisingEvents = true,
				StartInfo = _processStartInfo
			};

			_systemProcess.Exited += (sender, args) => Killed?.Invoke(this, args);
		}

		private int GetTimeoutInMiliseconds() => _timeoutSeconds * 1000;
	}
}
