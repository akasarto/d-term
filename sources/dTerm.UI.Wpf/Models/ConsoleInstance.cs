using dTerm.Core.Entities;
using dTerm.Core.Processes;
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

		public event EventHandler Started;
		public event EventHandler Killed;

		public string Name
		{
			get => _name;
			set => Set(ref _name, value);
		}

		public int PorcessId => _systemProcess?.Id ?? 0;

		public bool ProcessIsStarted => !(_systemProcess?.HasExited ?? true);

		public IntPtr ProcessMainHandle => _systemProcess?.Handle ?? IntPtr.Zero;

		public IntPtr ProcessMainWindowHandle => _systemProcess?.MainWindowHandle ?? IntPtr.Zero;

		public ConsoleType Type
		{
			get => _consoleType;
			set => Set(ref _consoleType, value);
		}

		public bool Start()
		{
			try
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
						return true;
					}
				}

				Kill();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return false;
		}

		public bool Kill()
		{
			try
			{
				if (!_systemProcess.HasExited)
				{
					_systemProcess.Kill();
					_systemProcess.WaitForExit(GetTimeoutInMiliseconds());
					return true;
				}

				_systemProcess = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return false;
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
