using System;
using System.Diagnostics;

namespace dTerm.Core.Processes
{
	public class TermConsoleProcess : IConsoleProcess
	{
		private ProcessStartInfo _processStartInfo;
		private int _timeoutSeconds;
		private Process _process;

		public TermConsoleProcess(ProcessStartInfo processStartInfo, int timeoutSeconds = 5)
		{
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(TermConsoleProcess));
			_timeoutSeconds = timeoutSeconds;

			ProcessSetup();
		}

		public event EventHandler Started;
		public event EventHandler Killed;

		public int Id => _process?.Id ?? 0;

		public bool IsRunning => !(_process?.HasExited ?? true);

		public IntPtr ProcessHandle => _process?.Handle ?? IntPtr.Zero;

		public IntPtr MainWindowHandle => _process?.MainWindowHandle ?? IntPtr.Zero;

		public bool Start()
		{
			try
			{
				var isStarted = false;
				var processStopwatch = Stopwatch.StartNew();
				var processTimeoutMiliseconds = GetTimeoutInMiliseconds();

				_process.Start();

				while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
				{
					System.Threading.Thread.Sleep(10);

					isStarted = _process?.MainWindowHandle != IntPtr.Zero && !_process.HasExited;

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
				if (!_process.HasExited)
				{
					_process.Kill();
					_process.WaitForExit(GetTimeoutInMiliseconds());
					return true;
				}

				_process = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return false;
		}

		private void ProcessSetup()
		{
			if (string.IsNullOrWhiteSpace(_processStartInfo.WorkingDirectory))
			{
				_processStartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			}

			_process = new Process()
			{
				EnableRaisingEvents = true,
				StartInfo = _processStartInfo
			};

			_process.Exited += (sender, args) => Killed?.Invoke(sender, args);
		}

		private int GetTimeoutInMiliseconds() => _timeoutSeconds * 1000;
	}
}
