using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Diagnostics;

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

		public event EventHandler<ProcessEventArgs> ProcessStatusChanged;

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

		public void Initialize()
		{
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = GetTimeoutInMiliseconds();

			_systemProcess.Start();

			while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
			{
				if (_systemProcess?.MainWindowHandle == IntPtr.Zero)
				{
					continue;
				}

				//User32Methods.ShowWindow(_systemProcess.MainWindowHandle, ShowWindowCommands.SW_HIDE);

				ProcessStatusChanged?.Invoke(this, new ProcessEventArgs(ProcessStatus.Initialized));
			}
		}

		public void Terminate()
		{
			if (!_systemProcess.HasExited)
			{
				_systemProcess.Kill();
				_systemProcess.WaitForExit(GetTimeoutInMiliseconds());
			}

			_systemProcess = null;
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

			_systemProcess.Exited += (sender, args) =>
			{
				var eArgs = new ProcessEventArgs(ProcessStatus.Terminated);

				ProcessStatusChanged?.Invoke(
					sender: this,
					e: eArgs
				);
			};
		}

		private int GetTimeoutInMiliseconds() => _timeoutSeconds * 1000;
	}
}
