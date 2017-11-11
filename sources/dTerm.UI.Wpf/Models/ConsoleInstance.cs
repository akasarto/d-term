using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Diagnostics;

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

		public int PorcessId => _systemProcess.Id;

		public IntPtr ProcessHandle => _systemProcess.Handle;

		public IntPtr ProcessMainWindowHandle => _systemProcess.MainWindowHandle;

		public ConsoleType Type => _consoleType;

		public void Initialize(Action<Process> onMainWindowHandleAccquiredAction = null)
		{
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = GetTimeoutInMiliseconds();

			_systemProcess.Start();

			while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
			{
				if (_systemProcess.MainWindowHandle != IntPtr.Zero)
				{
					onMainWindowHandleAccquiredAction?.Invoke(_systemProcess);
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

		private int GetTimeoutInMiliseconds() => _timeoutSeconds * 1000;
	}
}
