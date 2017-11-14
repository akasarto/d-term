using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace dTerm.Core.ProcessTaskKillers
{
	public class ConsoleTaskKiller
	{
		private List<int> _consoleProcessIds = new List<int>();

		public void AddProcessId(int consoleProcessId) => _consoleProcessIds.Add(consoleProcessId);

		public void Execute(bool throwOnEmpty = true)
		{
			if (_consoleProcessIds.Count <= 0)
			{
				if (throwOnEmpty)
				{
					throw new InvalidOperationException("No process ids provided.");
				}

				return;
			}

			//TODO: Review
			var taskKillThread = new Thread(new ThreadStart(() =>
			{
				using (var process = new Process())
				{
					var pids = _consoleProcessIds.Distinct().ToList();

					var args = $"{string.Join("", pids.Select(pid => $"/PID {pid} "))} /T /F";

					process.StartInfo = new ProcessStartInfo()
					{
						FileName = "taskkill",
						ErrorDialog = false,
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						CreateNoWindow = true,
						Arguments = args
					};

					process.Start();

					var output = process.StandardOutput.ReadToEnd();
					var error = process.StandardError.ReadToEnd();

					process.WaitForExit();
				}
			}));

			taskKillThread.Start();
		}

		public static ConsoleTaskKiller Create() => new ConsoleTaskKiller();
	}
}
